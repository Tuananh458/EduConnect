using Blazored.LocalStorage;
using EduConnect.Shared.DTOs.Auth;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EduConnect.Client.Services
{
    /// <summary>
    /// Quản lý token, hồ sơ và trạng thái đăng nhập người dùng (LocalStorage)
    /// </summary>
    public class UserSessionService
    {
        private readonly ILocalStorageService _storage;
        private readonly IHttpClientFactory _factory;

        private const string AccessKey = "access_token";
        private const string RefreshKey = "refresh_token";
        private const string ProfileKey = "user_profile";
        private const string HocSinhIdKey = "hoc_sinh_id";

        public UserSessionService(ILocalStorageService storage, IHttpClientFactory factory)
        {
            _storage = storage;
            _factory = factory;
        }

        // =============================================================
        // 🔹 TOKEN HANDLING
        // =============================================================

        public async Task SetTokensAsync(string access, string refresh)
        {
            // ✅ Lưu token trước
            await _storage.SetItemAsync(AccessKey, access);
            await _storage.SetItemAsync(RefreshKey, refresh);

            // ✅ Giải mã & lưu profile (nhưng KHÔNG gọi API phụ)
            await SaveUserProfileAsync(access);

            Console.WriteLine("[UserSessionService] 💾 Tokens saved successfully.");
        }

        public async Task<(string? Access, string? Refresh)> GetTokensAsync()
        {
            var access = await _storage.GetItemAsync<string>(AccessKey);
            var refresh = await _storage.GetItemAsync<string>(RefreshKey);
            return (access, refresh);
        }

        public async Task<string?> GetAccessTokenAsync() =>
            await _storage.GetItemAsync<string>(AccessKey);

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetAccessTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        // =============================================================
        // 🔹 JWT → PROFILE
        // =============================================================

        public async Task SaveUserProfileAsync(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length < 2) return;

                // 🧩 Giải mã payload của JWT
                var payload = parts[1]
                    .PadRight(parts[1].Length + (4 - parts[1].Length % 4) % 4, '=')
                    .Replace('-', '+').Replace('_', '/');

                var jsonBytes = Convert.FromBase64String(payload);
                var json = System.Text.Encoding.UTF8.GetString(jsonBytes);
                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                // ✅ Lấy avatar hiện có trong localStorage (nếu có)
                var oldProfile = await _storage.GetItemAsync<UserProfileDto>("user_profile");

                // 🔹 Lấy UserId từ các key phổ biến: "sub", "id", "userId"
                var idClaim = claims?.GetValueOrDefault("sub")?.ToString()
                             ?? claims?.GetValueOrDefault("id")?.ToString()
                             ?? claims?.GetValueOrDefault("userId")?.ToString();

                Guid.TryParse(idClaim, out var parsedUserId);

                // ✅ Tạo profile đầy đủ
                var profile = new UserProfileDto
                {
                    UserId = parsedUserId, // 🆕 Lưu đúng Guid người dùng
                    FullName = claims?.GetValueOrDefault("name")?.ToString() ?? "",
                    Email = claims?.GetValueOrDefault("email")?.ToString() ?? "",
                    Username = claims?.GetValueOrDefault("unique_name")?.ToString() ?? "",
                    Role = claims?.GetValueOrDefault("role")?.ToString() ?? "",
                    Avatar = oldProfile?.Avatar ?? "images/default-user.png" // ✅ Giữ avatar cũ nếu có
                };

                await _storage.SetItemAsync("user_profile", profile);
                Console.WriteLine($"[UserSessionService] 👤 Profile saved. Id={profile.UserId}, Role={profile.Role}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserSessionService.SaveUserProfileAsync] Decode error: {ex.Message}");
            }
        }


        public async Task<UserProfileDto?> GetProfileAsync() =>
            await _storage.GetItemAsync<UserProfileDto>(ProfileKey);

        // =============================================================
        // 🔹 HOCSINH ID
        // =============================================================

        public async Task FetchAndSaveHocSinhIdAsync()
        {
            try
            {
                var token = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(token)) return;

                var client = _factory.CreateClient("ApiClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var res = await client.GetAsync("api/HocSinh/me");
                if (!res.IsSuccessStatusCode) return;

                var idStr = await res.Content.ReadAsStringAsync();
                if (int.TryParse(idStr, out var id))
                    await _storage.SetItemAsync(HocSinhIdKey, id);

                Console.WriteLine($"[UserSessionService] 🎓 HocSinhId = {idStr}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserSessionService.FetchAndSaveHocSinhIdAsync] Error: {ex.Message}");
            }
        }

        // =============================================================
        // 🔹 TOKEN REFRESH
        // =============================================================

        private static bool _isRefreshing = false;

        public async Task<bool> TryRefreshTokenAsync()
        {
            if (_isRefreshing)
                return false;

            _isRefreshing = true;
            try
            {
                var tokens = await GetTokensAsync();
                if (string.IsNullOrEmpty(tokens.Refresh))
                    return false;

                var client = _factory.CreateClient("ApiClient");
                var response = await client.PostAsJsonAsync("api/auth/refresh", new
                {
                    refreshToken = tokens.Refresh
                });

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[AuthHandler] Refresh failed ({response.StatusCode})");
                    return false;
                }

                var envelope = await response.Content
                    .ReadFromJsonAsync<AuthService.ApiEnvelope<AuthService.TokenResponse>>();

                if (envelope?.Data == null)
                    return false;

                // 🔄 Lưu lại token mới
                await SetTokensAsync(envelope.Data.Access, envelope.Data.Refresh);

                Console.WriteLine("[UserSessionService] ✅ Token refreshed successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserSessionService.TryRefreshTokenAsync] Error: {ex.Message}");
                return false;
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        // =============================================================
        // 🔹 LOGOUT
        // =============================================================

        public async Task LogoutAsync()
        {
            await _storage.RemoveItemAsync(AccessKey);
            await _storage.RemoveItemAsync(RefreshKey);
            await _storage.RemoveItemAsync(ProfileKey);
            await _storage.RemoveItemAsync(HocSinhIdKey);
            Console.WriteLine("[UserSessionService] 🔒 Logged out & cleared localStorage");
        }
        // =============================================================
        // 🔹 HOCSINH ID (THÊM 3 HÀM NÀY)
        // =============================================================

        public async Task SetHocSinhIdAsync(int id)
        {
            await _storage.SetItemAsync(HocSinhIdKey, id);
        }

        public async Task<int?> GetHocSinhIdAsync()
        {
            return await _storage.GetItemAsync<int?>(HocSinhIdKey);
        }

        // =============================================================
        // 🔹 PROFILE (bổ sung cho ProfileAccount.razor)
        // =============================================================
        public async Task SetProfileAsync(UserProfileDto profile)
        {
            await _storage.SetItemAsync(ProfileKey, profile);
        }
        // =============================================================
        // 🔹 LẤY THÔNG TIN NGƯỜI DÙNG HIỆN TẠI (UserProfileDto)
        // =============================================================
        public async Task<UserProfileDto?> GetUserAsync()
        {
            try
            {
                var profile = await _storage.GetItemAsync<UserProfileDto>(ProfileKey);
                return profile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserSessionService.GetUserAsync] Error: {ex.Message}");
                return null;
            }
        }

    }
}
