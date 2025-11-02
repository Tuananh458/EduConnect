using System.Net.Http.Json;

namespace EduConnect.Client.Services
{
    public class HocLieuCauHoiService
    {
        private readonly HttpClient _http;
        public HocLieuCauHoiService(HttpClient http) => _http = http;

        public async Task<bool> AddAsync(int maHocLieu, int maCauHoi, double diem = 1)
        {
            var res = await _http.PostAsJsonAsync("HocLieuCauHoi", new { MaHocLieu = maHocLieu, MaCauHoi = maCauHoi, Diem = diem });
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int maHocLieu, int maCauHoi)
        {
            var res = await _http.DeleteAsync($"HocLieuCauHoi/{maHocLieu}/{maCauHoi}");
            return res.IsSuccessStatusCode;
        }
    }
}
