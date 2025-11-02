using EduConnect.Models.HocLieu;

namespace EduConnect.Repositories.Interfaces
{
    public interface IHocLieuRepository
    {
        Task<IEnumerable<HocLieu>> GetAllAsync();
        Task<HocLieu?> GetByIdAsync(int id);
        Task AddAsync(HocLieu entity);
        Task UpdateAsync(HocLieu entity);
        Task DeleteAsync(HocLieu entity);
    }
}
