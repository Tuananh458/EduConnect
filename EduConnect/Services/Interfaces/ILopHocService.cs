using EduConnect.Shared.DTOs.LopHoc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduConnect.Services.Interfaces
{
    public interface ILopHocService
    {
        // ✅ Dành cho Admin hoặc hệ thống: lấy tất cả lớp học
        Task<IEnumerable<LopHocDto>> GetAllAsync();

        // ✅ Dành cho giáo viên: chỉ lấy lớp của chính họ
        Task<IEnumerable<LopHocDto>> GetAllByNguoiTaoAsync(Guid nguoiTaoId);

        // ✅ Lấy chi tiết lớp
        Task<LopHocDto?> GetByIdAsync(int id);

        // ✅ Tạo lớp mới (có người tạo)
        Task CreateAsync(CreateLopHocRequest request);

        // ✅ Cập nhật lớp
        Task UpdateAsync(UpdateLopHocRequest request);

        // ✅ Xóa lớp
        Task DeleteAsync(int id, Guid nguoiTaoId);

    }
}
