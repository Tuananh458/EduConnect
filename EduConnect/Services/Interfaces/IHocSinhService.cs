using EduConnect.Shared.DTOs.HocSinh;
using EduConnect.Shared.DTOs.LopHoc;

namespace EduConnect.Services.Interfaces
{
    public interface IHocSinhService
    {
        Task<List<HocSinhDto>> GetByLopAsync(int maLopHoc);
        Task<bool> CreateAsync(CreateHocSinhRequest request);
        Task<bool> UpdateAsync(UpdateHocSinhRequest request);
        Task<bool> DeleteAsync(int maHocSinh);
        Task<ImportConfirmResult> ConfirmImportAsync(ImportConfirmRequest req);
        Task<ImportPreviewResponse> GeneratePreviewAsync(IFormFile file, int maLopHoc, string? prefix = null, string? defaultPassword = null);
        Task<int> DeleteManyAsync(List<int> ids);
        Task<HocSinhDto?> GetByUserIdAsync(Guid maNguoiDung);
        Task<LopHocChiTietDto?> GetLopHocCuaToiAsync(Guid maNguoiDung);


    }
}
