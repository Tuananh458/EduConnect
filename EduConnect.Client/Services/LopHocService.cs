using System.Net.Http.Json;
using EduConnect.Shared.DTOs.LopHoc;

namespace EduConnect.Client.Services
{
    public class LopHocService
    {
        private readonly HttpClient _http;

        public LopHocService(HttpClient http)
        {
            _http = http;
        }

        // ====================== GET ALL ======================
        public async Task<List<LopHocDto>> GetAllAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<LopHocDto>>("api/LopHoc")
                       ?? new List<LopHocDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LopHocService.GetAllAsync] Error: {ex.Message}");
                return new List<LopHocDto>();
            }
        }

        // ====================== GET BY ID ======================
        public async Task<LopHocDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<LopHocDto>($"api/LopHoc/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LopHocService.GetByIdAsync] Error: {ex.Message}");
                return null;
            }
        }

        // ====================== CREATE ======================
        // POST: https://localhost:7276/api/LopHoc
        public async Task<bool> CreateAsync(CreateLopHocRequest request, Guid nguoiTaoId)
        {
            try
            {
                // ✅ Gửi kèm NguoiTaoId (server sẽ lưu vào bảng LOPHOC)
                var payload = new
                {
                    request.MaKhoiHoc,
                    request.TenLopHoc,
                    NguoiTaoId = nguoiTaoId
                };

                var res = await _http.PostAsJsonAsync("api/LopHoc", payload);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LopHocService.CreateAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== UPDATE ======================
        public async Task<bool> UpdateAsync(UpdateLopHocRequest request)
        {
            try
            {
                var res = await _http.PutAsJsonAsync($"api/LopHoc/{request.MaLopHoc}", request);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LopHocService.UpdateAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== DELETE ======================
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var res = await _http.DeleteAsync($"api/LopHoc/{id}");
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LopHocService.DeleteAsync] Error: {ex.Message}");
                return false;
            }
        }
    }
}
