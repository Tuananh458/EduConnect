using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EduConnect.Client.Services
{
    /// <summary>
    /// HttpMessageHandler tự động gắn JWT và tự refresh khi hết hạn (401).
    /// </summary>
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly UserSessionService _session;

        public AuthMessageHandler(UserSessionService session)
        {
            _session = session;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // ===== 1️⃣ Gắn access token hiện tại =====
            var token = await _session.GetAccessTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // ===== 2️⃣ Gửi request gốc =====
            var response = await base.SendAsync(request, cancellationToken);

            // ===== 3️⃣ Nếu 401, thử refresh token =====
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Tránh vòng lặp vô hạn nếu refresh thất bại
                response.Dispose();

                bool refreshed = await _session.TryRefreshTokenAsync();
                if (refreshed)
                {
                    var newToken = await _session.GetAccessTokenAsync();
                    if (string.IsNullOrEmpty(newToken))
                        return response;

                    // ✅ Clone request (do HttpContent chỉ đọc 1 lần)
                    var clonedRequest = await CloneHttpRequestAsync(request);
                    clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                    // 🔁 Gửi lại request sau khi refresh thành công
                    response = await base.SendAsync(clonedRequest, cancellationToken);
                }
            }

            return response;
        }

        /// <summary>
        /// Clone lại HttpRequestMessage (phòng trường hợp content đã bị tiêu thụ).
        /// </summary>
        private static async Task<HttpRequestMessage> CloneHttpRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            // Clone headers
            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            // Clone content
            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}
