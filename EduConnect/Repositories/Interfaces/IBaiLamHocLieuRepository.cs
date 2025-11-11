using EduConnect.Models.HocLieu;

namespace EduConnect.Repositories.Interfaces
{
    public interface IBaiLamHocLieuRepository
    {
        Task<IEnumerable<BaiLamHocLieu>> GetAllAsync();
        Task<IEnumerable<BaiLamHocLieu>> GetByHocLieuAsync(int hocLieuId);
        Task<IEnumerable<BaiLamHocLieu>> GetByHocSinhAsync(int hocSinhId);
        Task<BaiLamHocLieu?> GetByIdAsync(int baiLamId);
        Task<BaiLamHocLieu> AddAsync(BaiLamHocLieu baiLam);
        Task UpdateAsync(BaiLamHocLieu baiLam);
        Task DeleteAsync(int baiLamId);
        Task<BaiLamHocLieu?> GetByHocSinhVaHocLieuAsync(int hocSinhId, int hocLieuId);
    }
}
