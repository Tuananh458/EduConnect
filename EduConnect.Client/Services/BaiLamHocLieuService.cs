using System.Net.Http.Json;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Client.Services
{
    public class BaiLamHocLieuService
    {
        private readonly HttpClient _http;

        public BaiLamHocLieuService(HttpClient http) => _http = http;

        public async Task<DeThiHocLieuDto?> GetDeThiAsync(int hocLieuId)
        {
            try { return await _http.GetFromJsonAsync<DeThiHocLieuDto>($"api/BaiLamHocLieu/dethi/{hocLieuId}"); }
            catch { return null; }
        }

        public async Task<int> TaoBaiLamAsync(TaoBaiLamRequest dto)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/BaiLamHocLieu/tao", dto);
                if (!res.IsSuccessStatusCode) return 0;
                var data = await res.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                return data is not null && data.TryGetValue("BaiLamId", out var id) ? id : 0;
            }
            catch { return 0; }
        }

        public async Task<bool> NopBaiAsync(NopBaiRequest dto)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/BaiLamHocLieu/nop", dto);
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<KetQuaBaiLamDto?> GetKetQuaAsync(int baiLamId)
        {
            try { return await _http.GetFromJsonAsync<KetQuaBaiLamDto>($"api/BaiLamHocLieu/{baiLamId}"); }
            catch { return null; }
        }

        public async Task<IEnumerable<BaiLamHocLieuDto>> GetKetQuaHocLieuAsync(int hocLieuId)
        {
            try
            {
                var rs = await _http.GetFromJsonAsync<IEnumerable<BaiLamHocLieuDto>>($"api/BaiLamHocLieu/ketqua/hoclieu/{hocLieuId}");
                return rs ?? Enumerable.Empty<BaiLamHocLieuDto>();
            }
            catch { return Enumerable.Empty<BaiLamHocLieuDto>(); }
        }
    }
}
