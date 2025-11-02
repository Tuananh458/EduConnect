using EduConnect.Models.HocLieu;

namespace EduConnect.Repositories.Interfaces
{
    public interface ICauHoiHocLieuRepository
    {
        Task<IEnumerable<CauHoiHocLieu>> GetAllAsync();
        Task<IEnumerable<CauHoiHocLieu>> GetByHocLieuAsync(int maHocLieu);
        Task<CauHoiHocLieu?> GetByIdAsync(int id);
        Task AddAsync(CauHoiHocLieu entity);
        Task DeleteAsync(CauHoiHocLieu entity);
    }
}
