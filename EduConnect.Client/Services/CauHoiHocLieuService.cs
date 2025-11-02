using System.Net.Http.Json;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Client.Services
{
    public class CauHoiHocLieuService
    {
        private readonly HttpClient _http;
        public CauHoiHocLieuService(HttpClient http) => _http = http;

        public async Task<List<CauHoiHocLieuDto>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<CauHoiHocLieuDto>>("CauHoiHocLieu") ?? new();

        public async Task<List<CauHoiHocLieuDto>> GetByHocLieuAsync(int maHocLieu)
            => await _http.GetFromJsonAsync<List<CauHoiHocLieuDto>>($"CauHoiHocLieu/byHocLieu/{maHocLieu}") ?? new();

        public async Task<bool> CreateAsync(CreateCauHoiHocLieuRequest req)
        {
            var res = await _http.PostAsJsonAsync("CauHoiHocLieu", req);
            return res.IsSuccessStatusCode;
        }
    }
}
