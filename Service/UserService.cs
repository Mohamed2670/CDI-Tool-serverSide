using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CDI_Tool.Dtos.UserDtos;
using CDI_Tool.Model;
using CDI_Tool.Repository;
using Microsoft.IdentityModel.Tokens;

namespace CDI_Tool.Service
{
    public class UserService(JwtOptions jwtOptions, UserRepository _userRepository)
    {
        internal async Task<bool> AddUser(UserAddDto userAddDto)
        {
            var result = true;
            result &= await _userRepository.GetUserByEmail(userAddDto.Email) == null;
            result &= await _userRepository.GetUserByUserName(userAddDto.UserName) == null;
            return result & await _userRepository.UserAdd(userAddDto);
        }
        internal async Task<(string accessToken, string refreshToken, string userId,string role)?> Login(UserLoginDto userLoginDto)
        {

            var user = await _userRepository.GetUserByEmail(userLoginDto.Email);
            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
            {
                return null;
            }


            var accessToken = TokenGenerate(user, expiresInMinutes: 100);
            // Refresh token now valid for 8 hours
            var refreshToken = TokenGenerate(user, expiresInMinutes: 480);
            var userId = user.Id.ToString();

            return (accessToken, refreshToken, userId,user.Role.ToString());
        }

        public string TokenGenerate(User user, int expiresInMinutes = 60, int expiresInDays = 0)
        {
            var expirationDate = DateTime.UtcNow.AddMinutes(expiresInMinutes);

            if (expiresInDays > 0)
            {
                expirationDate = DateTime.UtcNow.AddDays(expiresInDays);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Role, user.Role.ToString()),
                }),
                Expires = expirationDate
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }
        public async Task<(string accessToken, string refreshToken, string userId)?> Refresh(string email)
        {
            Console.WriteLine("email : " + email);
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            var accessToken = TokenGenerate(user, expiresInMinutes: 100);
            // Refresh token now valid for 8 hours
            var refreshToken = TokenGenerate(user, expiresInMinutes: 480);
            var userId = user.Id.ToString();
            return (accessToken, refreshToken, userId);
        }
    }
}