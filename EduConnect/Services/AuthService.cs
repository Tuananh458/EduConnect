using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EduConnect.Data;
using EduConnect.Models;
using EduConnect.Shared.DTOs.Auth;
using Google.Apis.Auth;

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

        public async Task<(string accessToken, string refreshToken)> RegisterAsync(
            string username, string fullName, string email, string password, string role)
        {
            if (await _db.NguoiDungs.AnyAsync(u => u.TenDangNhap == username))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");
            if (await _db.NguoiDungs.AnyAsync(u => u.Email == email))
                throw new InvalidOperationException("Email đã được sử dụng.");

            var nguoiDung = new NguoiDung
            {
                TenDangNhap = username,
                HoTen = fullName,
                Email = email,
                MatKhauHash = _hasher.HashPassword(null!, password),
                VaiTro = role,
                TrangThai = 1,
                AnhDaiDien = "/template/img/avt.svg",
                NgayTao = DateTime.UtcNow
            };

            _db.NguoiDungs.Add(nguoiDung);
            await _db.SaveChangesAsync();

            switch (role.ToLower())
            {
                case "student":
                case "hocsinh":
                    _db.HocSinhs.Add(new HocSinh
                    {
                        MaNguoiDung = nguoiDung.MaNguoiDung,
                        NgayTao = DateTime.UtcNow
                    });
                    break;

                case "teacher":
                case "giaovien":
                    _db.GiaoViens.Add(new GiaoVien
                    {
                        MaNguoiDung = nguoiDung.MaNguoiDung,
                        NgayTao = DateTime.UtcNow
                    });
                    break;

                case "parent":
                case "phuhuynh":
                    _db.PhuHuynhs.Add(new PhuHuynh
                    {
                        MaNguoiDung = nguoiDung.MaNguoiDung,
                        NgayTao = DateTime.UtcNow
                    });
                    break;

                default:
                    _db.HocSinhs.Add(new HocSinh
                    {
                        MaNguoiDung = nguoiDung.MaNguoiDung,
                        NgayTao = DateTime.UtcNow
                    });
                    break;
            }

            await _db.SaveChangesAsync();

            var (access, _) = _tokens.CreateAccessToken(nguoiDung);
            var refresh = _tokens.CreateRefreshToken();

            var days = int.Parse(_cfg["Jwt:RefreshTokenDays"] ?? "7");
            _db.TokenLamMois.Add(new TokenLamMoi
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(days)
            });

            await _db.SaveChangesAsync();
            return (access, refresh);
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(string emailOrUsername, string password)
        {
            var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(u =>
                u.Email == emailOrUsername || u.TenDangNhap == emailOrUsername);

            if (nguoiDung == null || _hasher.VerifyHashedPassword(null!, nguoiDung.MatKhauHash, password) != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Sai thông tin đăng nhập");

            nguoiDung.LanDangNhapCuoi = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var (access, _) = _tokens.CreateAccessToken(nguoiDung);
            var refresh = _tokens.CreateRefreshToken();

            var days = int.Parse(_cfg["Jwt:RefreshTokenDays"] ?? "7");
            _db.TokenLamMois.Add(new TokenLamMoi
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(days)
            });

            await _db.SaveChangesAsync();
            return (access, refresh);
        }

        public async Task<UserProfileDto?> GetProfileAsync(Guid id)
        {
            var nd = await _db.NguoiDungs.FindAsync(id);
            if (nd == null) return null;

            return new UserProfileDto
            {
                UserId = nd.MaNguoiDung,
                Username = nd.TenDangNhap,
                FullName = nd.HoTen,
                Email = nd.Email,
                Role = nd.VaiTro,
                Avatar = nd.AnhDaiDien
            };
        }

        public async Task UpdateProfileAsync(Guid id, UpdateProfileDto dto)
        {
            var nd = await _db.NguoiDungs.FirstOrDefaultAsync(u => u.MaNguoiDung == id)
                     ?? throw new Exception("Không tìm thấy tài khoản");

            if (!string.IsNullOrWhiteSpace(dto.FullName))
                nd.HoTen = dto.FullName.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != nd.Email)
            {
                bool exists = await _db.NguoiDungs.AnyAsync(u => u.Email == dto.Email && u.MaNguoiDung != id);
                if (exists)
                    throw new InvalidOperationException("Email đã được sử dụng bởi người khác.");

                nd.Email = dto.Email.Trim();
            }

            if (!string.IsNullOrWhiteSpace(dto.Avatar))
                nd.AnhDaiDien = dto.Avatar.Trim();

            await _db.SaveChangesAsync();
        }

        public async Task<(string accessToken, string refreshToken)> GoogleLoginAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var email = payload.Email?.Trim()?.ToLower();
            var fullName = payload.Name ?? "";
            var avatar = payload.Picture ?? "/template/img/avt.svg";

            if (string.IsNullOrEmpty(email))
                throw new InvalidOperationException("Không thể xác định email từ Google token.");

            // 🔍 1. Tìm xem user đã tồn tại chưa
            var user = await _db.NguoiDungs.FirstOrDefaultAsync(u => u.Email == email);

            // 🧱 2. Nếu chưa có thì tạo mới
            if (user == null)
            {
                user = new NguoiDung
                {
                    TenDangNhap = email.Split('@')[0],
                    HoTen = fullName,
                    Email = email,
                    AnhDaiDien = avatar,
                    TrangThai = 1,
                    VaiTro = "HocSinh", // Có thể mặc định hoặc tùy frontend gửi
                    NguonXacThuc = "Google",
                    NgayTao = DateTime.UtcNow
                };

                _db.NguoiDungs.Add(user);
                await _db.SaveChangesAsync();
            }
            else
            {
                // 🧠 Nếu user đã có, cập nhật ảnh hoặc tên mới nếu có thay đổi
                bool updated = false;
                if (string.IsNullOrWhiteSpace(user.AnhDaiDien) || user.AnhDaiDien == "/template/img/avt.svg")
                {
                    user.AnhDaiDien = avatar;
                    updated = true;
                }
                if (string.IsNullOrWhiteSpace(user.HoTen) && !string.IsNullOrWhiteSpace(fullName))
                {
                    user.HoTen = fullName;
                    updated = true;
                }
                if (string.IsNullOrEmpty(user.NguonXacThuc))
                {
                    user.NguonXacThuc = "Google";
                    updated = true;
                }
                if (updated) await _db.SaveChangesAsync();
            }

            // 🧩 3. Đảm bảo có bản ghi phụ tương ứng role
            switch (user.VaiTro.ToLower())
            {
                case "hocsinh":
                case "student":
                    if (!await _db.HocSinhs.AnyAsync(x => x.MaNguoiDung == user.MaNguoiDung))
                        _db.HocSinhs.Add(new HocSinh { MaNguoiDung = user.MaNguoiDung, NgayTao = DateTime.UtcNow });
                    break;

                case "giaovien":
                case "teacher":
                    if (!await _db.GiaoViens.AnyAsync(x => x.MaNguoiDung == user.MaNguoiDung))
                        _db.GiaoViens.Add(new GiaoVien { MaNguoiDung = user.MaNguoiDung, NgayTao = DateTime.UtcNow });
                    break;

                case "phuhuynh":
                case "parent":
                    if (!await _db.PhuHuynhs.AnyAsync(x => x.MaNguoiDung == user.MaNguoiDung))
                        _db.PhuHuynhs.Add(new PhuHuynh { MaNguoiDung = user.MaNguoiDung, NgayTao = DateTime.UtcNow });
                    break;

                default:
                    _db.HocSinhs.Add(new HocSinh { MaNguoiDung = user.MaNguoiDung, NgayTao = DateTime.UtcNow });
                    break;
            }
            await _db.SaveChangesAsync();

            // 🔄 4. Cập nhật lần đăng nhập
            user.LanDangNhapCuoi = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // 🔐 5. Sinh token đăng nhập
            var (access, _) = _tokens.CreateAccessToken(user);
            var refresh = _tokens.CreateRefreshToken();

            var days = int.Parse(_cfg["Jwt:RefreshTokenDays"] ?? "7");
            _db.TokenLamMois.Add(new TokenLamMoi
            {
                MaNguoiDung = user.MaNguoiDung,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(days)
            });

            await _db.SaveChangesAsync();
            return (access, refresh);
        }

    }
}
