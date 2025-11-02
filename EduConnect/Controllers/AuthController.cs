using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EduConnect.Data;
using EduConnect.Models.Auth;
using EduConnect.Services;
using EduConnect.Helpers;
using EduConnect.Hubs;
using Microsoft.AspNetCore.WebUtilities;
using Google.Apis.Auth;
using Microsoft.SqlServer.Server;
using EduConnect.Shared.DTOs;

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

        // ============================
        // 🔹 Helper xử lý lỗi model
        // ============================
        private IActionResult ValidationError()
        {
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new { field = e.Key, message = e.Value.Errors.First().ErrorMessage })
                .ToList();

            return BadRequest(ApiResponse.Error("Dữ liệu không hợp lệ", errors));
        }
        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest req)
        {
            try
            {
                // ✅ Xác thực token từ Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(req.IdToken);

                // Kiểm tra xem user đã tồn tại chưa
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

                if (user == null)
                {
                    user = new User
                    {
                        Username = payload.Email.Split('@')[0],
                        FullName = payload.Name ?? payload.Email,
                        Email = payload.Email,
                        AuthProvider = "Google",
                        Status = 1,
                        Role = "Student",
                        PasswordHash = "" // Không cần mật khẩu
                    };

                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                }

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

                return Ok(new
                {
                    status = "success",
                    message = "Đăng nhập Google thành công",
                    access,
                    refresh
                });
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(new { status = "error", message = "Google token không hợp lệ" });
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
                var (access, refresh) = await _auth.RegisterAsync(req.Username, req.FullName, req.Email, req.Password);

                await _hub.Clients.All.SendAsync("Notify", new { type = "info", message = $"Tài khoản {req.Username} vừa đăng ký thành công" });
                return Ok(ApiResponse.Success(new { access, refresh }, "Đăng ký thành công"));
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
                return Ok(ApiResponse.Success(new { access, refresh }, "Đăng nhập thành công"));
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

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null)
                return Ok(ApiResponse.Success(message: "Nếu email tồn tại, hướng dẫn đã được gửi."));
            var token = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());
            var pr = new PasswordReset
            {
                UserId = user.UserId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

            _db.PasswordResets.Add(pr);
            await _db.SaveChangesAsync();

            var frontend = _cfg["Mail:FrontendBaseUrl"];
            var link = $"{frontend}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            await _email.SendAsync(user.Email, "[EduConnect] Đặt lại mật khẩu",
                $"<p>Chào {user.FullName},</p><p>Bấm vào liên kết để đặt lại mật khẩu:</p><p><a href='{link}'>Đặt lại mật khẩu</a></p>");

            await _hub.Clients.All.SendAsync("Notify", new { type = "info", message = $"Người dùng {user.Email} yêu cầu đặt lại mật khẩu" });

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

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null)
                return BadRequest(ApiResponse.Error("Email không hợp lệ"));

            // ✅ Không decode token nữa — so sánh trực tiếp
            var pr = await _db.PasswordResets
                .Where(x => x.UserId == user.UserId && x.Token == req.Token && !x.Used && x.ExpiresAt >= DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (pr == null)
                return BadRequest(ApiResponse.Error("Token không hợp lệ hoặc đã hết hạn"));

            user.PasswordHash = _hasher.HashPassword(null!, req.NewPassword);
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(ApiResponse.Error("Không xác định được người dùng."));

            var uid = Guid.Parse(userId);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == uid);
            if (user == null)
                return NotFound(ApiResponse.Error("Không tìm thấy người dùng."));

            var verify = _hasher.VerifyHashedPassword(null!, user.PasswordHash, req.OldPassword);
            if (verify != PasswordVerificationResult.Success && verify != PasswordVerificationResult.SuccessRehashNeeded)
                return BadRequest(ApiResponse.Error("Mật khẩu cũ không đúng"));

            user.PasswordHash = _hasher.HashPassword(null!, req.NewPassword);
            await _db.SaveChangesAsync();

            await _hub.Clients.User(userId).SendAsync("Notify", new { type = "success", message = "Bạn đã đổi mật khẩu thành công" });

            return Ok(ApiResponse.Success(message: "Đổi mật khẩu thành công"));
        }
    }
}
