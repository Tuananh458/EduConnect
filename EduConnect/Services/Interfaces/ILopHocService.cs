using EduConnect.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduConnect.Services.Interfaces
{
    public interface ILopHocService
    {
        Task<IEnumerable<LopHocDto>> GetAllAsync();
        Task<LopHocDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateLopHocRequest request);
        Task UpdateAsync(UpdateLopHocRequest request);
        Task DeleteAsync(int id);
    }
}
