using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Interfaces
{
    public interface ICauHoiHocLieuService
    {
        Task<IEnumerable<CauHoiHocLieuDto>> GetAllAsync();
        Task<IEnumerable<CauHoiHocLieuDto>> GetByHocLieuAsync(int maHocLieu);
        Task<int> CreateAsync(CreateCauHoiHocLieuRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
