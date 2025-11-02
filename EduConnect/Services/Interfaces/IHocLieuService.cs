using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Interfaces
{
    public interface IHocLieuService
    {
        Task<IEnumerable<HocLieuDto>> GetAllAsync();
        Task<HocLieuDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateHocLieuRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
