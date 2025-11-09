using EduConnect.Shared.DTOs.HocLieu;
using System.Net.Http.Json;

namespace EduConnect.Client.Services
{
    public class HocLieuCauHoiService
    {
        private readonly HttpClient _http;

        public HocLieuCauHoiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<int>> GetSelectedAsync(int hocLieuId)
        {
            return await _http.GetFromJsonAsync<List<int>>($"api/HocLieuCauHoi/{hocLieuId}") ?? new();
        }

        public async Task SaveAsync(HocLieuCauHoiRequest req)
        {
            await _http.PostAsJsonAsync("api/HocLieuCauHoi/save", req);
        }
    }
}
