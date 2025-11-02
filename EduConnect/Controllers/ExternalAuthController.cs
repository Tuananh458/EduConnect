using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EduConnect.Data;
using EduConnect.Models;
using EduConnect.Services;
using EduConnect.Models.Auth;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalAuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokens;

        public ExternalAuthController(AppDbContext db, ITokenService tokens)
        {
            _db = db; _tokens = tokens;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var props = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleCallback), new { returnUrl }) };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authResult.Succeeded) return BadRequest("Google login failed");

            var email = authResult.Principal.FindFirstValue(ClaimTypes.Email);
            var name = authResult.Principal.Identity?.Name ?? email;

            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Email not provided");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = email,
                    FullName = name ?? email,
                    AuthProvider = "Google",
                    MustChangePassword = false,
                    Status = 1
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            var (access, _) = _tokens.CreateAccessToken(user);
            var refresh = _tokens.CreateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });
            await _db.SaveChangesAsync();

            // Trả về theo kiểu frontend đọc được (querystring / fragment / html)
            var frontend = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host; // hoặc lấy từ config
            var redirect = $"{frontend}/oauth-success?accessToken={Uri.EscapeDataString(access)}&refreshToken={Uri.EscapeDataString(refresh)}";
            return Redirect(redirect);
        }

        [HttpGet("microsoft")]
        public IActionResult MicrosoftLogin(string? returnUrl = null)
        {
            var props = new AuthenticationProperties { RedirectUri = Url.Action(nameof(MicrosoftCallback), new { returnUrl }) };
            return Challenge(props, MicrosoftAccountDefaults.AuthenticationScheme);
        }

        [HttpGet("microsoft-callback")]
        public async Task<IActionResult> MicrosoftCallback(string? returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync(MicrosoftAccountDefaults.AuthenticationScheme);
            if (!authResult.Succeeded) return BadRequest("Microsoft login failed");

            var email = authResult.Principal.FindFirstValue(ClaimTypes.Email)
                        ?? authResult.Principal.FindFirstValue("preferred_username");
            var name = authResult.Principal.Identity?.Name ?? email;
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Email not provided");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = email,
                    FullName = name ?? email,
                    AuthProvider = "Microsoft",
                    MustChangePassword = false,
                    Status = 1
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            var (access, _) = _tokens.CreateAccessToken(user);
            var refresh = _tokens.CreateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });
            await _db.SaveChangesAsync();

            var frontend = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var redirect = $"{frontend}/oauth-success?accessToken={Uri.EscapeDataString(access)}&refreshToken={Uri.EscapeDataString(refresh)}";
            return Redirect(redirect);
        }
    }
}
