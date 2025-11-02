using System.Net.Http.Json;
using EduConnect.Shared.DTOs;

namespace EduConnect.Client.Services
{
    public class LopHocService
    {
        private readonly HttpClient _http;

        public LopHocService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<LopHocDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<LopHocDto>>("LopHoc")
                   ?? new List<LopHocDto>();
        }

        public async Task<LopHocDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<LopHocDto>($"LopHoc/{id}");
        }

        public async Task<bool> CreateAsync(CreateLopHocRequest request)
        {
            var response = await _http.PostAsJsonAsync("LopHoc", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(UpdateLopHocRequest request)
        {
            var response = await _http.PutAsJsonAsync($"LopHoc/{request.MaLopHoc}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"LopHoc/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
