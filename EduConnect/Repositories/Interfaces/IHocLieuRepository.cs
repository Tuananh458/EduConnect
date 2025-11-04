using System.Collections.Generic;
using System.Threading.Tasks;
using EduConnect.Models.HocLieu;

namespace EduConnect.Repositories.Interfaces
{
    public interface IHocLieuRepository
    {
        Task<List<HocLieu>> GetAllAsync(
            string? keyword = null,
            string? maLoai = null,
            string? nguonTao = null,
            bool? laTuDo = null,
            bool? laAn = null);

        Task<HocLieu?> GetByIdAsync(int id);
        Task<HocLieu> AddAsync(HocLieu entity);
        Task UpdateAsync(HocLieu entity);
        Task DeleteAsync(HocLieu entity);
    }
}
