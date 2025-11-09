using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Client.Services
{
    public class HocLieuService
    {
        private readonly HttpClient _http;

        public HocLieuService(HttpClient http)
        {
            _http = http;
        }

        // ====================== GET ALL ======================
        // GET: https://localhost:7276/api/HocLieu
        public async Task<List<HocLieuListDto>?> GetAllAsync(HocLieuFilterDto filter)
        {
            try
            {
                var query = $"?Keyword={filter.Keyword}&MaLoaiHocLieu={filter.MaLoaiHocLieu}" +
                            $"&NguonTao={filter.NguonTao}&LaHocLieuTuDo={filter.LaHocLieuTuDo}" +
                            $"&LaHocLieuAn={filter.LaHocLieuAn}";

                return await _http.GetFromJsonAsync<List<HocLieuListDto>>($"api/HocLieu{query}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.GetAllAsync] Error: {ex.Message}");
                return null;
            }
        }

        // ====================== GET BY ID ======================
        // GET: https://localhost:7276/api/HocLieu/{id}
        public async Task<HocLieuDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<HocLieuDto>($"api/HocLieu/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.GetByIdAsync] Error: {ex.Message}");
                return null;
            }
        }

        // ====================== CREATE ======================
        // POST: https://localhost:7276/api/HocLieu
        public async Task<HocLieuDto?> CreateAsync(CreateHocLieuRequest dto)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/HocLieu", dto);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<HocLieuDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.CreateAsync] Error: {ex.Message}");
                return null;
            }
        }

        // ====================== DELETE ======================
        // DELETE: https://localhost:7276/api/HocLieu/{id}
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var res = await _http.DeleteAsync($"api/HocLieu/{id}");
                res.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.DeleteAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== DUPLICATE ======================
        // POST: https://localhost:7276/api/HocLieu/{id}/duplicate
        public async Task<HocLieuDto?> DuplicateAsync(int id)
        {
            try
            {
                var res = await _http.PostAsync($"api/HocLieu/{id}/duplicate", null);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<HocLieuDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.DuplicateAsync] Error: {ex.Message}");
                return null;
            }
        }

        // ====================== ASSIGN TO COURSE ======================
        // POST: https://localhost:7276/api/HocLieu/{id}/assign/{courseId}
        public async Task<bool> AssignToCourseAsync(int id, int courseId)
        {
            try
            {
                var res = await _http.PostAsync($"api/HocLieu/{id}/assign/{courseId}", null);
                res.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.AssignToCourseAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== SHARE ======================
        // POST: https://localhost:7276/api/HocLieu/{id}/share
        public async Task<bool> ShareAsync(int id)
        {
            try
            {
                var res = await _http.PostAsync($"api/HocLieu/{id}/share", null);
                res.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.ShareAsync] Error: {ex.Message}");
                return false;
            }
        }

        // ====================== LẤY CÂU HỎI TRONG HỌC LIỆU ======================
        // GET: https://localhost:7276/api/HocLieu/GetCauHoiTrongHocLieu/{hocLieuId}
        public async Task<List<CauHoiHocLieuDto>> GetCauHoiTrongHocLieu(int hocLieuId)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<CauHoiHocLieuDto>>($"api/HocLieu/GetCauHoiTrongHocLieu/{hocLieuId}")
                       ?? new List<CauHoiHocLieuDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.GetCauHoiTrongHocLieu] Error: {ex.Message}");
                return new List<CauHoiHocLieuDto>();
            }
        }
    }
}
