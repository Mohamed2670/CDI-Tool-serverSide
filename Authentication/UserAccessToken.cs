using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CDI_Tool.Dtos.TokenDtos;
using Microsoft.IdentityModel.Tokens;

namespace CDI_Tool.Authentication
{
    public class UserAccessToken(IHttpContextAccessor _httpContextAccessor, JwtOptions _jwtOptions)
    {
        public TokenReadDto ValidateRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false, // Since it's a refresh token
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var role = principal.FindFirst(ClaimTypes.Role)?.Value;
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                return new TokenReadDto { UserId = userId, UserRole = role, Email = email };
            }
            catch
            {
                return null;
            }
        }
    }
}