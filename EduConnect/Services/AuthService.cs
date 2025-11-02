using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EduConnect.Data;
using EduConnect.Models;
using EduConnect.Models.Auth;

namespace EduConnect.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<object> _hasher;
        private readonly ITokenService _tokens;
        private readonly IConfiguration _cfg;

        public AuthService(AppDbContext db, IPasswordHasher<object> hasher, ITokenService tokens, IConfiguration cfg)
        {
            _db = db;
            _hasher = hasher;
            _tokens = tokens;
            _cfg = cfg;
        }

        public async Task<(string accessToken, string refreshToken)> RegisterAsync(string username, string fullName, string email, string password)
        {
            if (await _db.Users.AnyAsync(u => u.Username == username))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại");
            if (await _db.Users.AnyAsync(u => u.Email == email))
                throw new InvalidOperationException("Email đã được sử dụng");

            var user = new User
            {
                Username = username,
                FullName = fullName,
                Email = email,
                PasswordHash = _hasher.HashPassword(null!, password),
                Role = "Student",
                Status = 0
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var (access, _) = _tokens.CreateAccessToken(user);
            var refresh = _tokens.CreateRefreshToken();

            var days = int.Parse(_cfg["Jwt:RefreshTokenDays"]!);
            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(days)
            });
            await _db.SaveChangesAsync();

            return (access, refresh);
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(string emailOrUsername, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Email == emailOrUsername || u.Username == emailOrUsername);

            if (user == null || _hasher.VerifyHashedPassword(null!, user.PasswordHash, password) != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Sai thông tin đăng nhập");

            var (access, _) = _tokens.CreateAccessToken(user);
            var refresh = _tokens.CreateRefreshToken();

            var days = int.Parse(_cfg["Jwt:RefreshTokenDays"]!);
            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(days)
            });
            await _db.SaveChangesAsync();

            return (access, refresh);
        }
    }
}
