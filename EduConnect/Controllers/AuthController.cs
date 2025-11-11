using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EduConnect.Data;
using EduConnect.Models;
using EduConnect.Services;
using EduConnect.Helpers;
using EduConnect.Hubs;
using Microsoft.AspNetCore.WebUtilities;
using Google.Apis.Auth;
using EduConnect.Shared.DTOs;
using EduConnect.Shared.DTOs.Auth;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _auth;
        private readonly ITokenService _tokens;
        private readonly IEmailService _email;
        private readonly IPasswordHasher<object> _hasher;
        private readonly IConfiguration _cfg;
        private readonly IHubContext<NotifyHub> _hub;

        public AuthController(
            AppDbContext db,
            IAuthService auth,
            ITokenService tokens,
            IEmailService email,
            IPasswordHasher<object> hasher,
            IConfiguration cfg,
            IHubContext<NotifyHub> hub)
        {
            _db = db;
            _auth = auth;
            _tokens = tokens;
            _email = email;
            _hasher = hasher;
            _cfg = cfg;
            _hub = hub;
        }

        private IActionResult ValidationError()
        {
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new { field = e.Key, message = e.Value.Errors.First().ErrorMessage })
                .ToList();

            return BadRequest(ApiResponse.Error("Dữ liệu không hợp lệ", errors));
        }

        // ============================
        // 🧩 Đăng nhập Google
        // ============================
        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest req)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _cfg["Auth:Google:ClientId"] }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(req.IdToken, settings);

                var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(u => u.Email == payload.Email);

                if (nguoiDung == null)
                {
                    var baseUsername = payload.Email.Split('@')[0];
                    var username = baseUsername;
                    int count = 1;
                    while (await _db.NguoiDungs.AnyAsync(u => u.TenDangNhap == username))
                        username = $"{baseUsername}{count++}";

                    nguoiDung = new NguoiDung
                    {
                        TenDangNhap = username,
                        HoTen = payload.Name ?? payload.Email,
                        Email = payload.Email,
                        NguonXacThuc = "Google",
                        TrangThai = 1,
                        VaiTro = "HocSinh",
                        MatKhauHash = string.Empty
                    };

                    _db.NguoiDungs.Add(nguoiDung);
                    await _db.SaveChangesAsync();

                    // 🧩 Tạo bản ghi phụ tương ứng theo vai trò
                    switch (nguoiDung.VaiTro.ToLower())
                    {
                        case "hocsinh":
                        case "student":
                            if (!await _db.HocSinhs.AnyAsync(h => h.MaNguoiDung == nguoiDung.MaNguoiDung))
                                _db.HocSinhs.Add(new HocSinh
                                {
                                    MaNguoiDung = nguoiDung.MaNguoiDung,
                                    NgayTao = DateTime.UtcNow
                                });
                            break;

                        case "giaovien":
                        case "teacher":
                            if (!await _db.GiaoViens.AnyAsync(g => g.MaNguoiDung == nguoiDung.MaNguoiDung))
                                _db.GiaoViens.Add(new GiaoVien
                                {
                                    MaNguoiDung = nguoiDung.MaNguoiDung,
                                    NgayTao = DateTime.UtcNow
                                });
                            break;

                        case "phuhuynh":
                        case "parent":
                            if (!await _db.PhuHuynhs.AnyAsync(p => p.MaNguoiDung == nguoiDung.MaNguoiDung))
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
                }
                else
                {
                    // ✅ Nếu user đã có rồi, đảm bảo vẫn có record phụ
                    switch (nguoiDung.VaiTro.ToLower())
                    {
                        case "hocsinh":
                        case "student":
                            if (!await _db.HocSinhs.AnyAsync(h => h.MaNguoiDung == nguoiDung.MaNguoiDung))
                                _db.HocSinhs.Add(new HocSinh { MaNguoiDung = nguoiDung.MaNguoiDung, NgayTao = DateTime.UtcNow });
                            break;
                        case "giaovien":
                        case "teacher":
                            if (!await _db.GiaoViens.AnyAsync(g => g.MaNguoiDung == nguoiDung.MaNguoiDung))
                                _db.GiaoViens.Add(new GiaoVien { MaNguoiDung = nguoiDung.MaNguoiDung, NgayTao = DateTime.UtcNow });
                            break;
                        case "phuhuynh":
                        case "parent":
                            if (!await _db.PhuHuynhs.AnyAsync(p => p.MaNguoiDung == nguoiDung.MaNguoiDung))
                                _db.PhuHuynhs.Add(new PhuHuynh { MaNguoiDung = nguoiDung.MaNguoiDung, NgayTao = DateTime.UtcNow });
                            break;
                    }

                    await _db.SaveChangesAsync();
                }

                // 🔐 Sinh token đăng nhập
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

                return Ok(ApiResponse.Success(new TokenResponse(access, refresh), "Đăng nhập Google thành công"));
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(ApiResponse.Error("Google token không hợp lệ"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Error("Lỗi xử lý Google login", ex.Message));
            }
        }


        // ============================
        // 🧩 Đăng ký
        // ============================
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationError();

            try
            {
                var (access, refresh) = await _auth.RegisterAsync(
                    req.Username,
                    req.FullName,
                    req.Email,
                    req.Password,
                    req.Role
                );

                await _hub.Clients.All.SendAsync("Notify", new
                {
                    type = "info",
                    message = $"Tài khoản {req.Username} vừa đăng ký thành công"
                });

                return Ok(ApiResponse.Success(new TokenResponse(access, refresh), "Đăng ký thành công"));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse.Error(ex.Message));
            }
        }

        // ============================
        // 🧩 Đăng nhập
        // ============================
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationError();

            try
            {
                var (access, refresh) = await _auth.LoginAsync(req.EmailOrUsername, req.Password);
                return Ok(ApiResponse.Success(new TokenResponse(access, refresh), "Đăng nhập thành công"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse.Error(ex.Message));
            }
        }

        // ============================
        // 🔹 Quên mật khẩu
        // ============================
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationError();

            var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (nguoiDung == null)
                return Ok(ApiResponse.Success(message: "Nếu email tồn tại, hướng dẫn đã được gửi."));

            var token = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());
            var datLai = new DatLaiMatKhau
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

            _db.DatLaiMatKhaus.Add(datLai);
            await _db.SaveChangesAsync();

            var frontend = _cfg["Mail:FrontendBaseUrl"];
            var link = $"{frontend}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(nguoiDung.Email)}";

            await _email.SendAsync(nguoiDung.Email, "[EduConnect] Đặt lại mật khẩu",
                $"<p>Chào {nguoiDung.HoTen},</p><p>Bấm vào liên kết để đặt lại mật khẩu:</p><p><a href='{link}'>Đặt lại mật khẩu</a></p>");

            await _hub.Clients.All.SendAsync("Notify", new
            {
                type = "info",
                message = $"Người dùng {nguoiDung.Email} yêu cầu đặt lại mật khẩu"
            });

            return Ok(ApiResponse.Success(message: "Nếu email tồn tại, hướng dẫn đã được gửi."));
        }

        // ============================
        // 🔹 Đặt lại mật khẩu
        // ============================
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationError();

            var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (nguoiDung == null)
                return BadRequest(ApiResponse.Error("Email không hợp lệ"));

            var pr = await _db.DatLaiMatKhaus
                .Where(x => x.MaNguoiDung == nguoiDung.MaNguoiDung && x.Token == req.Token && !x.Used && x.ExpiresAt >= DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (pr == null)
                return BadRequest(ApiResponse.Error("Token không hợp lệ hoặc đã hết hạn"));

            nguoiDung.MatKhauHash = _hasher.HashPassword(null!, req.NewPassword);
            pr.Used = true;
            await _db.SaveChangesAsync();

            return Ok(ApiResponse.Success(message: "Đặt lại mật khẩu thành công"));
        }

        // ============================
        // 🔹 Đổi mật khẩu
        // ============================
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationError();

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                return Unauthorized(ApiResponse.Error("Không xác định được người dùng."));

            var guid = Guid.Parse(id);
            var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(u => u.MaNguoiDung == guid);
            if (nguoiDung == null)
                return NotFound(ApiResponse.Error("Không tìm thấy người dùng."));

            var verify = _hasher.VerifyHashedPassword(null!, nguoiDung.MatKhauHash, req.OldPassword);
            if (verify != PasswordVerificationResult.Success)
                return BadRequest(ApiResponse.Error("Mật khẩu cũ không đúng"));

            nguoiDung.MatKhauHash = _hasher.HashPassword(null!, req.NewPassword);
            await _db.SaveChangesAsync();

            await _hub.Clients.User(id).SendAsync("Notify", new
            {
                type = "success",
                message = "Bạn đã đổi mật khẩu thành công"
            });

            return Ok(ApiResponse.Success(message: "Đổi mật khẩu thành công"));
        }

        // ============================
        // 🧩 Lấy hồ sơ
        // ============================
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                return Unauthorized(ApiResponse.Error("Không xác định được người dùng."));

            var profile = await _auth.GetProfileAsync(Guid.Parse(id));
            return Ok(ApiResponse.Success(profile));
        }

        // ============================
        // ✏️ Cập nhật hồ sơ
        // ============================
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _auth.UpdateProfileAsync(Guid.Parse(id!), dto);
            return Ok(ApiResponse.Success(message: "Cập nhật thành công"));
        }

        // ============================
        // 📤 Upload Avatar
        // ============================
        [Authorize]
        [HttpPost("upload-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse.Error("File không hợp lệ"));

            var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var nguoiDung = await _db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null) return NotFound(ApiResponse.Error("Không tìm thấy người dùng"));

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(file.FileName);
            var filename = $"{id}{ext}";
            var path = Path.Combine(folder, filename);

            await using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var avatarUrl = $"{baseUrl}/uploads/avatars/{filename}";

            nguoiDung.AnhDaiDien = avatarUrl;
            await _db.SaveChangesAsync();

            return Ok(ApiResponse.Success(new { avatar = avatarUrl }, "Đổi avatar thành công"));
        }

        // ============================
        // 🔁 Làm mới Access Token
        // ============================
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.RefreshToken))
                return BadRequest(ApiResponse.Error("Thiếu refresh token."));

            var tokenEntity = await _db.TokenLamMois
                .Include(x => x.NguoiDung)
                .FirstOrDefaultAsync(x => x.Token == req.RefreshToken);

            if (tokenEntity == null || !tokenEntity.IsActive)
                return Unauthorized(ApiResponse.Error("Token không hợp lệ hoặc đã hết hạn."));

            var nguoiDung = tokenEntity.NguoiDung;
            if (nguoiDung == null)
                return Unauthorized(ApiResponse.Error("Không tìm thấy người dùng."));

            tokenEntity.RevokedAt = DateTime.UtcNow;

            var (access, _) = _tokens.CreateAccessToken(nguoiDung);
            var refresh = _tokens.CreateRefreshToken();

            _db.TokenLamMois.Add(new TokenLamMoi
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(_cfg["Jwt:RefreshTokenDays"] ?? "7"))
            });

            await _db.SaveChangesAsync();

            return Ok(ApiResponse.Success(new TokenResponse(access, refresh), "Làm mới token thành công"));
        }

        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; } = string.Empty;
        }
    }
}
