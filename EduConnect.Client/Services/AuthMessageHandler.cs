using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EduConnect.Client.Services
{
    /// <summary>
    /// Gắn Bearer token cho mọi request.
    /// Nếu 401, thử refresh 1 lần rồi gửi lại.
    /// </summary>
    public sealed class AuthMessageHandler : DelegatingHandler
    {
        private readonly UserSessionService _session;

        public AuthMessageHandler(UserSessionService session)
        {
            _session = session;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1) Gắn access token hiện tại (nếu có)
            await AttachAccessTokenAsync(request);

            // 2) Gửi request lần 1
            var response = await base.SendAsync(request, cancellationToken);

            // 3) Nếu 401 và có refresh token → thử refresh 1 lần
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var tokens = await _session.GetTokensAsync();
                if (!string.IsNullOrWhiteSpace(tokens.Refresh))
                {
                    var refreshed = await TryRefreshAsync(cancellationToken);
                    if (refreshed)
                    {
                        // Gắn lại token mới và gửi lại request
                        response.Dispose();
                        var clone = await CloneRequestAsync(request);
                        await AttachAccessTokenAsync(clone);
                        return await base.SendAsync(clone, cancellationToken);
                    }
                    else
                    {
                        // Refresh thất bại → đăng xuất sạch
                        await _session.LogoutAsync();
                    }
                }
            }

            return response;
        }

        private async Task AttachAccessTokenAsync(HttpRequestMessage request)
        {
            var (access, _) = await _session.GetTokensAsync();
            if (!string.IsNullOrWhiteSpace(access))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);
            }
        }

        private async Task<bool> TryRefreshAsync(CancellationToken ct)
        {
            try
            {
                var (_, refresh) = await _session.GetTokensAsync();
                if (string.IsNullOrWhiteSpace(refresh)) return false;

                using var refreshRequest = new HttpRequestMessage(HttpMethod.Post, "api/auth/refresh")
                {
                    Content = new StringContent($$"""{"refreshToken":"{{refresh}}"}""", System.Text.Encoding.UTF8, "application/json")
                };

                // Gửi refresh bằng chính HttpClient (qua same handler chain)
                var refreshResponse = await base.SendAsync(refreshRequest, ct);
                if (!refreshResponse.IsSuccessStatusCode) return false;

                var envelope = await refreshResponse.Content.ReadFromJsonAsync<ApiEnvelope<TokenResponse>>();
                if (envelope?.Data == null) return false;

                await _session.SetTokensAsync(envelope.Data.Access, envelope.Data.Refresh);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Clone request để gửi lại (không dùng lại instance cũ)
        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            // Copy headers
            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            // Copy content (nếu có)
            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // Properties/options
            clone.Version = request.Version;
#if NET7_0_OR_GREATER
            clone.VersionPolicy = request.VersionPolicy;
#endif
            return clone;
        }

        // Khớp với định dạng API envelope phía client đang dùng
        private sealed record ApiEnvelope<T>(string? Status, string? Message, T? Data);
        private sealed record TokenResponse(string Access, string Refresh);
    }
}
