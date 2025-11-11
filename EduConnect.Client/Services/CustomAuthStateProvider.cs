using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace EduConnect.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly UserSessionService _session;

        public CustomAuthStateProvider(UserSessionService session)
        {
            _session = session;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // 🟢 Lấy access token từ session
            var token = await _session.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                // Chưa đăng nhập
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                // 🟢 Giải mã claim trong token JWT
                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            catch
            {
                // Token lỗi → đăng xuất
                await _session.LogoutAsync();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        // ✅ Khi user login (Google / thường)
        public async Task MarkUserAsAuthenticated()
        {
            // Đảm bảo token đã lưu xong trước khi đọc
            await Task.Delay(100); // đợi một nhịp nhỏ để sync storage

            var token = await _session.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
                return;
            }

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            Console.WriteLine("[AuthStateProvider] User marked as authenticated.");
        }

        // ✅ Khi user logout
        public void MarkUserAsLoggedOut()
        {
            var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(anonymous));

            Console.WriteLine("[AuthStateProvider] User logged out.");
        }

        // ✅ Giải mã claim từ JWT
        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var parts = jwt.Split('.');
            if (parts.Length < 2) return claims;

            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var json = System.Text.Encoding.UTF8.GetString(jsonBytes);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            if (keyValuePairs == null) return claims;

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Key is "role" or ClaimTypes.Role)
                {
                    if (kvp.Value is JsonElement je && je.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var role in je.EnumerateArray())
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, kvp.Value?.ToString() ?? ""));
                    }
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
                }
            }

            return claims;
        }
    }
}
