using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EduConnect.Models.Auth;

namespace EduConnect.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _cfg;
        public TokenService(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public (string accessToken, DateTime expiresAt) CreateAccessToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User object cannot be null when creating token.");

            // Đảm bảo không null để tránh lỗi ArgumentNullException
            var userId = user.UserId.ToString();
            var username = user.Username;
            var email = user.Email ?? string.Empty;
            var role = user.Role ?? "User"; // Mặc định "User" nếu chưa có

            var jwt = _cfg.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new Exception("Missing Jwt:Key in configuration")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var expiresMinutes = int.TryParse(jwt["AccessTokenMinutes"], out var minutes) ? minutes : 60;
            var expires = DateTime.UtcNow.AddMinutes(minutes);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (accessToken, expires);
        }

        public string CreateRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
