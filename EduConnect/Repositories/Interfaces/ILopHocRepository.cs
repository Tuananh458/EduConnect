using EduConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduConnect.Repositories.Interfaces
{
    public interface ILopHocRepository
    {
        Task<IEnumerable<LopHoc>> GetAllAsync();
        Task<LopHoc?> GetByIdAsync(int id);
        Task AddAsync(LopHoc entity);
        Task UpdateAsync(LopHoc entity);
        Task DeleteAsync(LopHoc entity);
        Task<bool> ExistsAsync(int id);
    }
}

