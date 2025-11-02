using System.Net.Http.Json;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Client.Services
{
    public class HocLieuService
    {
        private readonly HttpClient _http;
        public HocLieuService(HttpClient http) => _http = http;

        public async Task<List<HocLieuDto>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<HocLieuDto>>("HocLieu") ?? new();

        public async Task<HocLieuDto?> GetByIdAsync(int id)
            => await _http.GetFromJsonAsync<HocLieuDto>($"HocLieu/{id}");

        public async Task<int?> CreateAsync(CreateHocLieuRequest req)
        {
            var res = await _http.PostAsJsonAsync("HocLieu", req);
            if (!res.IsSuccessStatusCode) return null;

            var json = await res.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (json != null && json.TryGetValue("maHocLieu", out var idObj))
            {
                return Convert.ToInt32(idObj);
            }
            return null;
        }
    }
}
