using EduConnect.Shared.DTOs.HocLieu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduConnect.Services.Interfaces
{
    public interface IHocLieuCauHoiService
    {
        Task<List<int>> GetSelectedCauHoiIdsAsync(int hocLieuId);
        Task SaveHocLieuCauHoiAsync(HocLieuCauHoiRequest request);
    }
}
