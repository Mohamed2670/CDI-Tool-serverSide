using CDI_Tool.Authentication;
using CDI_Tool.Dtos.UserDtos;
using CDI_Tool.Model;
using CDI_Tool.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CDI_Tool.Controller
{
    [ApiController, Route("user")]
    public class UserController(UserService _userService, UserAccessToken userAccessToken) : ControllerBase
    {
        [HttpPost("guest-login"),AllowAnonymous]
        public IActionResult GuestLogin([FromBody] GuestLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Guest name is required");

            // Create minimal user model for token
            var user = new User
            {
                UserName = request.Name,
                Email = "", // guests don’t need email
                Role = Role.User
            };

            var accessToken = _userService.TokenGenerate(user); 

            return Ok(new
            {
                accessToken,
                role = user.Role.ToString()
            });
        }

        [HttpPost("register"),AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] UserAddDto userAddDto)
        {
            var user = await _userService.AddUser(userAddDto);
            if (user == false)
            {
                return BadRequest("User Name of Email Already exists");
            }
            return Ok("User Added");
        }
        [HttpPost("login"),AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var tokens = await _userService.Login(userLoginDto);
            if (tokens == null)
                return Unauthorized(new { message = "Invalid email or password" });
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevent access from JavaScript
                Secure = true, // Use only on HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddDays(1) // Expiration time
            };
            Response.Cookies.Append("refreshToken", tokens.Value.refreshToken, cookieOptions);

            return Ok(new { accessToken = tokens.Value.accessToken, tokens.Value.role });
        }
        [HttpGet("token-test")]
        [Authorize]
        public IActionResult TokenTest()
        {
            return Ok("Authorized");
        }
        [HttpPost("access-token")]
        public async Task<IActionResult> GenerateToken()
        {
            // Get refresh token from cookies
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token found");
            }

            // Validate and extract user from refresh token
            Console.WriteLine("refresh token: " + refreshToken);
            var user = userAccessToken.ValidateRefreshToken(refreshToken);
            if (user == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            // Generate new access & refresh tokens
            var tokens = await _userService.Refresh(user.Email);
            if (tokens == null)
            {
                return BadRequest("Failed to refresh token");
            }

            // Set new refresh token in secure cookies
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevent JavaScript access
                Secure = true, // HTTPS only
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddDays(1) // Expiration time
            };
            Response.Cookies.Append("refreshToken", tokens.Value.refreshToken, cookieOptions);
            return Ok(new
            {
                accessToken = tokens.Value.accessToken
            });
        }
    }
}