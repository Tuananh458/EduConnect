using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Client.Services
{
    public class CauHoiHocLieuService
    {
        private readonly HttpClient _http;

        public CauHoiHocLieuService(HttpClient http)
        {
            _http = http;
        }

        // GET: api/CauHoiHocLieu?hocLieuId=1
        public async Task<List<CauHoiHocLieuDto>> GetByHocLieuAsync(int hocLieuId)
        {
            try
            {
                var data = await _http.GetFromJsonAsync<List<CauHoiHocLieuDto>>($"CauHoiHocLieu?hocLieuId={hocLieuId}");
                return data ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CauHoiHocLieuService.GetByHocLieuAsync] Error: {ex.Message}");
                return new();
            }
        }

        // GET: api/CauHoiHocLieu/{id}
        public async Task<CauHoiHocLieuDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<CauHoiHocLieuDto>($"CauHoiHocLieu/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CauHoiHocLieuService.GetByIdAsync] Error: {ex.Message}");
                return null;
            }
        }

        // POST: api/CauHoiHocLieu
        public async Task<CauHoiHocLieuDto?> CreateAsync(CreateCauHoiHocLieuRequest dto)
        {
            var res = await _http.PostAsJsonAsync("CauHoiHocLieu", dto);
            if (!res.IsSuccessStatusCode)
            {
                var msg = await res.Content.ReadAsStringAsync();
                throw new Exception($"Tạo câu hỏi thất bại: {msg}");
            }

            return await res.Content.ReadFromJsonAsync<CauHoiHocLieuDto>();
        }

        // PUT: api/CauHoiHocLieu/{id}
        public async Task UpdateAsync(int id, CreateCauHoiHocLieuRequest dto)
        {
            var res = await _http.PutAsJsonAsync($"CauHoiHocLieu/{id}", dto);
            if (!res.IsSuccessStatusCode)
            {
                var msg = await res.Content.ReadAsStringAsync();
                throw new Exception($"Cập nhật câu hỏi thất bại: {msg}");
            }
        }

        // DELETE: api/CauHoiHocLieu/{id}
        public async Task DeleteAsync(int id)
        {
            var res = await _http.DeleteAsync($"CauHoiHocLieu/{id}");
            if (!res.IsSuccessStatusCode)
            {
                var msg = await res.Content.ReadAsStringAsync();
                throw new Exception($"Xóa câu hỏi thất bại: {msg}");
            }
        }
    }
}
