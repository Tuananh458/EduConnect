using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace EduConnect.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly UserSessionService _session;
        private const string AccessKey = "access_token";

        public CustomAuthStateProvider(ILocalStorageService localStorage, UserSessionService session)
        {
            _localStorage = localStorage;
            _session = session;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // 🟢 Đọc token từ session service (ưu tiên token mới nhất)
            var token = await _session.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        // ✅ Gọi khi user login
        public async Task MarkUserAsAuthenticated()
        {
            var state = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }

        // ✅ Gọi khi user logout
        public void MarkUserAsLoggedOut()
        {
            var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
        }

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
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(System.Text.Encoding.UTF8.GetString(jsonBytes));
            if (keyValuePairs == null) return claims;

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Key is "role" or ClaimTypes.Role)
                {
                    if (kvp.Value is JsonElement je && je.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var r in je.EnumerateArray())
                            claims.Add(new Claim(ClaimTypes.Role, r.ToString()));
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
