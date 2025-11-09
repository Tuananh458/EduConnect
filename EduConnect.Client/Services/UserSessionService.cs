using Blazored.LocalStorage;
using EduConnect.Shared.DTOs.Auth;
using System.Net.Http.Json;
using System.Text.Json;

namespace EduConnect.Client.Services
{
    public class UserSessionService
    {
        private readonly ILocalStorageService _storage;
        private readonly IHttpClientFactory _factory; // 🔹 Dùng factory để tạo client tạm
        private const string AccessKey = "access_token";
        private const string RefreshKey = "refresh_token";
        private const string ProfileKey = "user_profile";

        public UserSessionService(ILocalStorageService storage, IHttpClientFactory factory)
        {
            _storage = storage;
            _factory = factory;
        }

        // ============================ TOKEN HANDLING ============================

        public async Task SetTokensAsync(string access, string refresh)
        {
            await _storage.SetItemAsync(AccessKey, access);
            await _storage.SetItemAsync(RefreshKey, refresh);

            // ✅ Tự động lưu profile từ AccessToken
            await SaveUserProfileAsync(access);
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
            var access = await _storage.GetItemAsync<string>(AccessKey);
            return !string.IsNullOrEmpty(access);
        }

        // ============================ JWT DECODE ============================

        public async Task SaveUserProfileAsync(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length < 2) return;

                string payload = parts[1];
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                payload = payload.Replace('-', '+').Replace('_', '/');
                var jsonBytes = Convert.FromBase64String(payload);
                var json = System.Text.Encoding.UTF8.GetString(jsonBytes);
                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                var existing = await GetProfileAsync();

                var profile = new UserProfileDto
                {
                    FullName = claims?.GetValueOrDefault("name")?.ToString() ?? "",
                    Email = claims?.GetValueOrDefault("email")?.ToString() ?? "",
                    Username = claims?.GetValueOrDefault("unique_name")?.ToString() ?? "",
                    Role = claims?.GetValueOrDefault("role")?.ToString() ?? "",
                    Avatar = existing?.Avatar ?? "images/default-user.png"
                };

                await _storage.SetItemAsync(ProfileKey, profile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserSessionService.SaveUserProfileAsync] Decode error: {ex.Message}");
            }
        }

        // ============================ PROFILE ============================

        public async Task SetProfileAsync(UserProfileDto profile)
        {
            await _storage.SetItemAsync(ProfileKey, profile);
        }

        public async Task<UserProfileDto?> GetProfileAsync()
        {
            return await _storage.GetItemAsync<UserProfileDto>(ProfileKey);
        }

        // ============================ TOKEN REFRESH ============================

        /// <summary>
        /// Làm mới Access Token khi hết hạn (gọi API /api/auth/refresh)
        /// </summary>
        public async Task<bool> TryRefreshTokenAsync()
        {
            try
            {
                var tokens = await GetTokensAsync();
                if (string.IsNullOrEmpty(tokens.Refresh))
                    return false;

                // ✅ Tạo HttpClient độc lập, không có AuthHandler để tránh vòng lặp 401
                var client = _factory.CreateClient("ApiClient");
                client.DefaultRequestHeaders.Authorization = null;

                var response = await client.PostAsJsonAsync("api/auth/refresh", new
                {
                    refreshToken = tokens.Refresh
                });

                if (!response.IsSuccessStatusCode)
                    return false;

                var json = await response.Content
                    .ReadFromJsonAsync<AuthService.ApiEnvelope<AuthService.TokenResponse>>();

                if (json?.Data == null) return false;

                await SetTokensAsync(json.Data.Access, json.Data.Refresh);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserSessionService.TryRefreshTokenAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ============================ LOGOUT ============================

        public async Task LogoutAsync()
        {
            await _storage.RemoveItemAsync(AccessKey);
            await _storage.RemoveItemAsync(RefreshKey);
            await _storage.RemoveItemAsync(ProfileKey);
        }
    }
}
