using System.Collections.Generic;
using System.Threading.Tasks;
using EduConnect.Models.HocLieu;

namespace EduConnect.Repositories.Interfaces
{
    public interface ICauHoiHocLieuRepository
    {
        Task<List<CauHoiHocLieu>> GetByHocLieuAsync(int hocLieuId);
        Task<CauHoiHocLieu?> GetByIdAsync(int id);
        Task<CauHoiHocLieu> AddAsync(CauHoiHocLieu entity);
        Task UpdateAsync(CauHoiHocLieu entity);
        Task DeleteAsync(CauHoiHocLieu entity);
    }
}
