using System.Collections.Generic;
using System.Threading.Tasks;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Interfaces
{
    public interface IHocLieuService
    {
        Task<List<HocLieuListDto>> GetAllAsync(HocLieuFilterDto filter);
        Task<HocLieuDto?> GetByIdAsync(int id);
        Task<HocLieuDto> CreateAsync(CreateHocLieuRequest request);
        Task<List<CauHoiHocLieuDto>> GetCauHoiTrongHocLieuAsync(int hocLieuId);

        Task<bool> DeleteAsync(int id);
    }
}
