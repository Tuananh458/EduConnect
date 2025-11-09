using System.Net.Http.Json;
using System.Text.Json;
using EduConnect.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EduConnect.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly UserSessionService _session;
        private readonly NavigationManager _nav;
        private readonly AuthenticationStateProvider _authProvider;

        public bool IsLoggedIn { get; private set; }
        public event Action? OnLoginStateChanged;

        public AuthService(HttpClient http, UserSessionService session, NavigationManager nav, AuthenticationStateProvider authProvider)
        {
            _http = http;
            _session = session;
            _nav = nav;
            _authProvider = authProvider;
        }

        // ======================== DTOs nội bộ ========================
        public record LoginRequest(string EmailOrUsername, string Password);
        public record RegisterRequest(string Username, string FullName, string Email, string Password);
        public record ApiEnvelope<T>(string? Status, string? Message, T? Data);
        public record TokenResponse(string Access, string Refresh);

        // ======================== LOGIN ========================
        public async Task<(bool success, string message)> LoginAsync(string emailOrUsername, string password)
        {
            try
            {
                var req = new LoginRequest(emailOrUsername, password);
                var res = await _http.PostAsJsonAsync("api/auth/login", req);

                if (!res.IsSuccessStatusCode)
                    return (false, await res.Content.ReadAsStringAsync());

                var json = await res.Content.ReadFromJsonAsync<ApiEnvelope<TokenResponse>>();
                if (json?.Data == null)
                    return (false, json?.Message ?? "Không nhận được dữ liệu đăng nhập");

                // Lưu token
                await _session.SetTokensAsync(json.Data.Access, json.Data.Refresh);

                // Lấy hồ sơ người dùng
                var profileRes = await _http.GetFromJsonAsync<ApiEnvelope<UserProfileDto>>("api/auth/me");
                if (profileRes?.Data != null)
                    await _session.SetProfileAsync(profileRes.Data);

                IsLoggedIn = true;
                OnLoginStateChanged?.Invoke();

                return (true, json.Message ?? "Đăng nhập thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối: {ex.Message}");
            }
        }

        // ======================== REGISTER ========================
        public async Task<(bool success, string message)> RegisterAsync(string username, string fullName, string email, string password)
        {
            try
            {
                var req = new RegisterRequest(username, fullName, email, password);
                var res = await _http.PostAsJsonAsync("api/auth/register", req);

                if (!res.IsSuccessStatusCode)
                    return (false, await res.Content.ReadAsStringAsync());

                var json = await res.Content.ReadFromJsonAsync<ApiEnvelope<TokenResponse>>();
                if (json?.Data != null)
                    await _session.SetTokensAsync(json.Data.Access, json.Data.Refresh);

                return (true, json?.Message ?? "Đăng ký thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối: {ex.Message}");
            }
        }

        // ======================== LOCAL PROFILE ========================
        public async Task<dynamic?> GetCurrentUserAsync() => await _session.GetProfileAsync();

        // ======================== LOGOUT ========================
        public async Task LogoutAsync()
        {
            await _session.LogoutAsync();
            IsLoggedIn = false;

            if (_authProvider is CustomAuthStateProvider custom)
                custom.MarkUserAsLoggedOut();

            OnLoginStateChanged?.Invoke();
            _nav.NavigateTo("/login", true);
        }

        // ======================== SYNC LOGIN STATE ========================
        public async Task LoginSyncAsync()
        {
            var tokens = await _session.GetTokensAsync();
            var accessToken = tokens.Access;
            IsLoggedIn = !string.IsNullOrWhiteSpace(accessToken);

            if (IsLoggedIn)
            {
                try
                {
                    var profile = await _session.GetProfileAsync();
                    if (profile == null)
                    {
                        var profileRes = await _http.GetFromJsonAsync<ApiEnvelope<UserProfileDto>>("api/auth/me");
                        if (profileRes?.Data != null)
                            await _session.SetProfileAsync(profileRes.Data);
                    }

                    if (_authProvider is CustomAuthStateProvider custom)
                        await custom.MarkUserAsAuthenticated();
                }
                catch { }
            }
            else
            {
                if (_authProvider is CustomAuthStateProvider custom)
                    custom.MarkUserAsLoggedOut();
            }
        }

        // ======================== RESET PASSWORD ========================
        public async Task<(bool, string)> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/auth/reset-password", new { Email = email, Token = token, NewPassword = newPassword });
                return res.IsSuccessStatusCode
                    ? (true, "✅ Mật khẩu đã được đặt lại thành công!")
                    : (false, await res.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // ======================== FORGOT PASSWORD ========================
        public async Task<(bool, string)> ForgotPasswordAsync(string email)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/auth/forgot-password", new { Email = email });
                return res.IsSuccessStatusCode
                    ? (true, "✅ Hướng dẫn đặt lại mật khẩu đã được gửi!")
                    : (false, await res.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // ======================== GET PROFILE ========================
        public async Task<UserProfileDto?> GetProfileAsync()
        {
            var res = await _http.GetFromJsonAsync<ApiEnvelope<UserProfileDto>>("api/auth/me");
            return res?.Data;
        }

        // ======================== UPDATE PROFILE ========================
        public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
        {
            var res = await _http.PutAsJsonAsync("api/auth/profile", dto);
            return res.IsSuccessStatusCode;
        }

        // ======================== UPLOAD AVATAR ========================
        public async Task<string?> UploadAvatarAsync(MultipartFormDataContent content)
        {
            try
            {
                var res = await _http.PostAsync("api/auth/upload-avatar", content);
                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[Avatar Upload Fail] {res.StatusCode}");
                    return null;
                }

                var json = await res.Content.ReadFromJsonAsync<ApiEnvelope<JsonElement>>();

                if (json?.Data.ValueKind == JsonValueKind.Object &&
                    json.Data.TryGetProperty("avatar", out var avatarProp))
                {
                    var avatarUrl = avatarProp.GetString();
                    Console.WriteLine($"[Avatar Upload OK] {avatarUrl}");
                    return avatarUrl;
                }

                Console.WriteLine("[Avatar Upload] Response không có trường 'avatar'");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Avatar Upload Error] {ex.Message}");
                return null;
            }
        }

        // ======================== STATE CHANGE NOTIFIER ========================
        public void NotifyLoginStateChanged()
        {
            try
            {
                Console.WriteLine("[AuthService] NotifyLoginStateChanged triggered");
                OnLoginStateChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService] NotifyLoginStateChanged error: {ex.Message}");
            }
        }
    }
}
