using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using EduConnect.Models.Auth;
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

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

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

                // ✅ Lưu token
                await _session.SetTokensAsync(json.Data.Access, json.Data.Refresh);

                // ✅ Gắn token vào HttpClient để các request sau có Authorization
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", json.Data.Access);

                // ⚠️ Bỏ dòng SaveUserProfileAsync vì chỉ lấy từ JWT (không có avatar)
                // await _session.SaveUserProfileAsync(json.Data.Access);

                // ✅ Gọi API lấy hồ sơ người dùng thật từ DB (bao gồm Avatar)
                var profileRes = await _http.GetFromJsonAsync<ApiEnvelope<UserProfileDto>>("api/auth/me");
                if (profileRes?.Data != null)
                {
                    await _session.SetProfileAsync(profileRes.Data);
                }

                // ✅ Cập nhật trạng thái đăng nhập
                IsLoggedIn = true;

                if (_authProvider is CustomAuthStateProvider custom)
                    await custom.MarkUserAsAuthenticated();

                // ✅ Gửi sự kiện để Topbar reload lại
                OnLoginStateChanged?.Invoke();

                return (true, json.Message ?? "Đăng nhập thành công");

            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối: {ex.Message}");
            }
        }

        // ======================== GOOGLE LOGIN ========================
        public async Task<(bool success, string message)> GoogleLoginAsync(string idToken)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/auth/google-login", new { IdToken = idToken });
                if (!res.IsSuccessStatusCode)
                {
                    var msg = await res.Content.ReadAsStringAsync();
                    return (false, $"Đăng nhập Google thất bại: {msg}");
                }

                var json = await res.Content.ReadFromJsonAsync<ApiEnvelope<TokenResponse>>();
                if (json?.Data == null)
                    return (false, "Không nhận được token từ server.");

                // ✅ Lưu token
                await _session.SetTokensAsync(json.Data.Access, json.Data.Refresh);

                // ✅ Gắn token vào HttpClient (bắt buộc, tránh lỗi 401)
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", json.Data.Access);

                // ⚠️ Bỏ dòng SaveUserProfileAsync vì chỉ đọc từ JWT (không có Avatar)
                // await _session.SaveUserProfileAsync(json.Data.Access);

                // ✅ Gọi API lấy hồ sơ người dùng thật từ DB (bao gồm Avatar)
                var profileRes = await _http.GetFromJsonAsync<ApiEnvelope<UserProfileDto>>("api/auth/me");
                if (profileRes?.Data != null)
                {
                    await _session.SetProfileAsync(profileRes.Data);
                    Console.WriteLine($"[GoogleLoginAsync] 👤 Đã nhận profile từ API: Avatar={profileRes.Data.Avatar}");
                }

                // ✅ Đồng bộ trạng thái
                IsLoggedIn = true;
                if (_authProvider is CustomAuthStateProvider custom)
                    await custom.MarkUserAsAuthenticated();

                OnLoginStateChanged?.Invoke();

                Console.WriteLine("[GoogleLoginAsync] ✅ Thành công, user đã đăng nhập.");
                return (true, json.Message ?? "Đăng nhập Google thành công");

            }
            catch (Exception ex)
            {
                return (false, $"Lỗi đăng nhập Google: {ex.Message}");
            }
        }

        // ======================== REGISTER ========================
        public async Task<(bool success, string message, Dictionary<string, string[]>? fieldErrors)> RegisterAsync(
            string username, string fullName, string email, string password, UserRole role = UserRole.HocSinh)
        {
            try
            {
                var req = new
                {
                    Username = username,
                    FullName = fullName,
                    Email = email,
                    Password = password,
                    Role = role.ToString()
                };

                var res = await _http.PostAsJsonAsync("api/auth/register", req);
                var content = await res.Content.ReadAsStringAsync();

                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadFromJsonAsync<ApiEnvelope<TokenResponse>>();
                    if (json?.Data != null)
                    {
                        await _session.SetTokensAsync(json.Data.Access, json.Data.Refresh);

                        // ✅ Gắn header để tránh lỗi khi gọi API sau đăng ký
                        _http.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", json.Data.Access);
                    }

                    return (true, json?.Message ?? "Đăng ký thành công!", null);
                }

                try
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(content, _jsonOptions);
                    var dict = error?.Data?.ToDictionary(
                        e => e.Field!,
                        e => new[] { e.Message! }
                    );
                    return (false, error?.Message ?? "Đăng ký thất bại", dict);
                }
                catch
                {
                    return (false, content, null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối: {ex.Message}", null);
            }
        }

        private class ApiErrorResponse
        {
            public string? Status { get; set; }
            public string? Message { get; set; }
            public List<FieldError>? Data { get; set; }
        }

        private class FieldError
        {
            public string? Field { get; set; }
            public string? Message { get; set; }
        }

        // ======================== LOGOUT ========================
        public async Task LogoutAsync()
        {
            await _session.LogoutAsync();
            IsLoggedIn = false;

            _http.DefaultRequestHeaders.Authorization = null;

            if (_authProvider is CustomAuthStateProvider custom)
                custom.MarkUserAsLoggedOut();

            OnLoginStateChanged?.Invoke();
            _nav.NavigateTo("/login", forceLoad: false);
        }

        // ======================== SYNC LOGIN STATE ========================
        public async Task LoginSyncAsync()
        {
            var tokens = await _session.GetTokensAsync();
            var accessToken = tokens.Access;

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                IsLoggedIn = true;
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);

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
                catch
                {
                    IsLoggedIn = false;
                }
            }
            else
            {
                IsLoggedIn = false;
                if (_authProvider is CustomAuthStateProvider custom)
                    custom.MarkUserAsLoggedOut();
            }

            OnLoginStateChanged?.Invoke();
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

        // ======================== PROFILE ========================
        public async Task<UserProfileDto?> GetProfileAsync()
        {
            var res = await _http.GetFromJsonAsync<ApiEnvelope<UserProfileDto>>("api/auth/me");
            return res?.Data;
        }

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
                    return null;

                var json = await res.Content.ReadFromJsonAsync<ApiEnvelope<JsonElement>>();
                if (json?.Data.ValueKind == JsonValueKind.Object &&
                    json.Data.TryGetProperty("avatar", out var avatarProp))
                {
                    return avatarProp.GetString();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Avatar Upload Error] {ex.Message}");
                return null;
            }
        }

        // ======================== STATE NOTIFIER ========================
        public void NotifyLoginStateChanged()
        {
            try
            {
                OnLoginStateChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService] NotifyLoginStateChanged error: {ex.Message}");
            }
        }
    }
}
