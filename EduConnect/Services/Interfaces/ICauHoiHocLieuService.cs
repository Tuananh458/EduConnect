using System.Collections.Generic;
using System.Threading.Tasks;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Interfaces
{
    public interface ICauHoiHocLieuService
    {
        Task<List<CauHoiHocLieuDto>> GetByHocLieuAsync(int hocLieuId);
        Task<CauHoiHocLieuDto?> GetByIdAsync(int id);
        Task<CauHoiHocLieuDto> CreateAsync(CreateCauHoiHocLieuRequest request);
        Task<bool> UpdateAsync(int id, CreateCauHoiHocLieuRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
