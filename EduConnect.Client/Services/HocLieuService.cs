using System.Collections.Generic;
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

        // GET https://localhost:7276/api/HocLieu
        public async Task<List<HocLieuListDto>?> GetAllAsync(HocLieuFilterDto filter)
        {
            // build query
            var query = $"?Keyword={filter.Keyword}&MaLoaiHocLieu={filter.MaLoaiHocLieu}&NguonTao={filter.NguonTao}&LaHocLieuTuDo={filter.LaHocLieuTuDo}&LaHocLieuAn={filter.LaHocLieuAn}";
            return await _http.GetFromJsonAsync<List<HocLieuListDto>>($"HocLieu{query}");
        }
        // GET: api/HocLieu/{id}
        public async Task<HocLieuDto?> GetByIdAsync(int id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<HocLieuDto>($"HocLieu/{id}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HocLieuService.GetByIdAsync] Error: {ex.Message}");
                return null;
            }
        }

        // POST https://localhost:7276/api/HocLieu
        public async Task<HocLieuDto?> CreateAsync(CreateHocLieuRequest dto)
        {
            var res = await _http.PostAsJsonAsync("HocLieu", dto);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<HocLieuDto>();
        }

        // DELETE https://localhost:7276/api/HocLieu/{id}
        public async Task DeleteAsync(int id)
        {
            var res = await _http.DeleteAsync($"HocLieu/{id}");
            res.EnsureSuccessStatusCode();
        }

        // POST https://localhost:7276/api/HocLieu/{id}/duplicate
        public async Task<HocLieuDto?> DuplicateAsync(int id)
        {
            var res = await _http.PostAsync($"HocLieu/{id}/duplicate", null);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<HocLieuDto>();
        }

        // POST https://localhost:7276/api/HocLieu/{id}/assign/{courseId}
        public async Task AssignToCourseAsync(int id, int courseId)
        {
            var res = await _http.PostAsync($"HocLieu/{id}/assign/{courseId}", null);
            res.EnsureSuccessStatusCode();
        }

        // POST https://localhost:7276/api/HocLieu/{id}/share
        public async Task ShareAsync(int id)
        {
            var res = await _http.PostAsync($"HocLieu/{id}/share", null);
            res.EnsureSuccessStatusCode();
        }
    }
}
