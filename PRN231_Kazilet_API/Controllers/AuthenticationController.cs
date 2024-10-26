using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_Kazilet_API.Models.Entities;
using PRN231_Kazilet_API.Services.Impl;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_Kazilet_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticationController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            if (await _userService.UserExists(email))
                return BadRequest("Email already exists.");

            User u = new User
            {
                Username = username,
                Email = email,
                Password = password,
                Role = 1,
                Type = "email",
            };

            var result = _userService.Register(u);
            return result > 0 ? Ok(result) : BadRequest("Register failed.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var authenticatedUser = await _userService.Authenticate(email, password);
            if (authenticatedUser == null)
                return Unauthorized();
            var token = GenerateJwtToken(authenticatedUser);
            return Ok(new { Token = token });
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Authentication");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (result.Succeeded)
            {
                var googleId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                var username = result.Principal.FindFirstValue(ClaimTypes.Name);
                User? u = _userService.GetUserGoogle(email, googleId);
                int uid;
                if (u == null){
                    u = new User
                    {
                        Username = username,
                        Email = email,
                        Password = googleId,
                        Role = 1,
                        Type = "google"
                    };
                    uid = _userService.RegisterGoogle(u);
                    u = _userService.GetUser(uid);
                }
                else
                {
                    uid = u.Id;
                }
                var token = GenerateJwtToken(u);

                // Lưu token vào cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true, 
                    SameSite = SameSiteMode.None, 
                    Expires = DateTimeOffset.UtcNow.AddSeconds(int.Parse(_configuration["Jwt:ExpireSeconds"]))
                };

                Response.Cookies.Append("accessToken", token, cookieOptions);
                return Redirect($"https://localhost:7081/gameplay/join");
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(User authenticatedUser)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, authenticatedUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, authenticatedUser.Username),
                new Claim("role", authenticatedUser.RoleNavigation.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(int.Parse(_configuration["Jwt:ExpireSeconds"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetValueFromJwtToken(string field, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var value = jwtToken.Claims.FirstOrDefault(c => c.Type == field);
            return value != null ? value.Value : "";
        }

        private User? GetUserFromJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var uid = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");

            if (usernameClaim == null || usernameClaim == null)
                return null;

            try
            {
                int userId = int.Parse(uid.Value);
                return _userService.GetUser(userId);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
