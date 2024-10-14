﻿using Microsoft.IdentityModel.Tokens;
using PRN231_Kazilet_API.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        private readonly PRN231_KaziletContext _context;
        
        public AuthService(IConfiguration configuration, PRN231_KaziletContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GetGameplayToken(string code, string username)
        {
            if (username != null)
            {
                var claims = new[]
                     {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Username", username),
                    new Claim("Code", code),
                    new Claim("Type", "GameplayToken"),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenValue;
            }
            else
            {
                return "";
            }
        }

        public string CheckGameplayCodeValid(string token)
        {
            string code = GetCodeFromToken(token);
            Console.WriteLine(code);
            if (_context.GameplaySettings.FirstOrDefault(g => (g.IsCompleted == false || g.IsCompleted == null) && g.Code == code) != null)
            {
                return code;
            }
            return "";
        }

        public string GetCodeFromToken(string token)
        {
            try
            {
                // Kiểm tra nếu token không hợp lệ
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new ArgumentException("Token không hợp lệ");
                }

                // Tạo một đối tượng JwtSecurityTokenHandler
                var handler = new JwtSecurityTokenHandler();

                // Đọc token
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                // Kiểm tra nếu token không phải là JwtSecurityToken
                if (jwtToken == null)
                {
                    throw new ArgumentException("Token không hợp lệ");
                }

                // Tìm kiếm claim với tên là "Code"
                var codeClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Code");

                // Nếu không tìm thấy claim, trả về null hoặc xử lý theo cách bạn muốn
                return codeClaim?.Value;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string GetUsernameFromToken(string token)
        {
            try
            {
                // Kiểm tra nếu token không hợp lệ
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new ArgumentException("Token không hợp lệ");
                }

                // Tạo một đối tượng JwtSecurityTokenHandler
                var handler = new JwtSecurityTokenHandler();

                // Đọc token
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                // Kiểm tra nếu token không phải là JwtSecurityToken
                if (jwtToken == null)
                {
                    throw new ArgumentException("Token không hợp lệ");
                }

                // Tìm kiếm claim với tên là "Code"
                var codeClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Username");

                // Nếu không tìm thấy claim, trả về null hoặc xử lý theo cách bạn muốn
                return codeClaim?.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
}
