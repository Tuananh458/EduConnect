using EduConnect.Shared.DTOs.HocSinh;
using EduConnect.Shared.DTOs.LopHoc;
using System.Net.Http.Json;

namespace EduConnect.Client.Services
{
    public class HocSinhService
    {
        private readonly HttpClient _http;

        public HocSinhService(HttpClient http)
        {
            _http = http;
        }

        // ====================== GET BY LỚP ======================
        // GET: https://localhost:7276/api/HocSinh/lop/{maLopHoc}
        public async Task<List<HocSinhDto>> GetByLopAsync(int maLopHoc)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<HocSinhDto>>($"api/HocSinh/lop/{maLopHoc}")
                       ?? new List<HocSinhDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocSinhService.GetByLopAsync] Error: {ex.Message}");
                return new List<HocSinhDto>();
            }
        }

        // ====================== CREATE ======================
        // POST: https://localhost:7276/api/HocSinh
        public async Task<bool> CreateAsync(CreateHocSinhRequest request)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/HocSinh", request);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocSinhService.CreateAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== UPDATE ======================
        // PUT: https://localhost:7276/api/HocSinh/{id}
        public async Task<bool> UpdateAsync(UpdateHocSinhRequest req)
        {
            try
            {
                var res = await _http.PutAsJsonAsync($"api/HocSinh/{req.MaHocSinh}", req);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocSinhService.UpdateAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== DELETE ======================
        // DELETE: https://localhost:7276/api/HocSinh/{id}
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var res = await _http.DeleteAsync($"api/HocSinh/{id}");
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocSinhService.DeleteAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== LỚP CỦA TÔI ======================
        // GET: https://localhost:7276/api/HocSinh/lop-cua-toi
        public async Task<LopHocChiTietDto?> GetLopCuaToiAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<LopHocChiTietDto>("api/HocSinh/lop-cua-toi");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[HocSinhService.GetLopCuaToiAsync] Network Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocSinhService.GetLopCuaToiAsync] Error: {ex.Message}");
                return null;
            }
        }
    }
}
