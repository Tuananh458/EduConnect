using EduConnect.Models.HocLieu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduConnect.Repositories.Interfaces
{
    public interface IHocLieuCauHoiRepository
    {
        Task<List<HocLieuCauHoi>> GetByHocLieuAsync(int hocLieuId);
        Task SaveAsync(int hocLieuId, List<int> cauHoiIds);
    }
}
