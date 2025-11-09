using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EduConnect.Data;
using EduConnect.Models.Auth;
using EduConnect.Shared.DTOs.Auth; // ✅ để dùng UserProfileDto & UpdateProfileDto

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

        // ================================
        // 🟢 Đăng ký
        // ================================
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
                Status = 1,
                Avatar = "/template/img/avt.svg" // ✅ thêm avatar mặc định
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

        // ================================
        // 🟢 Đăng nhập
        // ================================
        public async Task<(string accessToken, string refreshToken)> LoginAsync(string emailOrUsername, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Email == emailOrUsername || u.Username == emailOrUsername);

            if (user == null || _hasher.VerifyHashedPassword(null!, user.PasswordHash, password) != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Sai thông tin đăng nhập");

            user.LastLoginAt = DateTime.UtcNow; // ✅ ghi lại thời gian đăng nhập
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

        // ================================
        // 📌 Lấy hồ sơ
        // ================================
        public async Task<UserProfileDto?> GetProfileAsync(Guid userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserProfileDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Avatar = user.Avatar
            };
        }

        // ================================
        // ✏️ Cập nhật hồ sơ
        // ================================
        public async Task UpdateProfileAsync(Guid userId, UpdateProfileDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId)
                       ?? throw new Exception("Không tìm thấy tài khoản");

            // ✅ Kiểm tra và cập nhật FullName
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                user.FullName = dto.FullName.Trim();

            // ✅ Kiểm tra và cập nhật Email nếu khác hiện tại
            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
            {
                bool exists = await _db.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != userId);
                if (exists)
                    throw new InvalidOperationException("Email đã được sử dụng bởi người khác.");

                user.Email = dto.Email.Trim();
            }

            // ✅ Cập nhật Avatar nếu có
            if (!string.IsNullOrWhiteSpace(dto.Avatar))
                user.Avatar = dto.Avatar.Trim();

            await _db.SaveChangesAsync();
        }

    }
}
