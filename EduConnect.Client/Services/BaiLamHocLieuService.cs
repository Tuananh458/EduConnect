using EduConnect.Shared.DTOs.HocLieu;
using System.Net.Http.Json;

namespace EduConnect.Client.Services
{
    public class BaiLamHocLieuService
    {
        private readonly HttpClient _http;
        public BaiLamHocLieuService(HttpClient http) => _http = http;

        public async Task<double> NopBaiAsync(BaiLamHocLieuRequest req)
        {
            var res = await _http.PostAsJsonAsync("api/BaiLamHocLieu/nopbai", req);
            var data = await res.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            return data != null && data.ContainsKey("tongDiem")
                ? Convert.ToDouble(data["tongDiem"])
                : 0;
        }
    }
}
