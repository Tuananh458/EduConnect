using EduConnect.Repositories.Interfaces;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Services.Implementations
{
    public class HocLieuCauHoiService : IHocLieuCauHoiService
    {
        private readonly IHocLieuCauHoiRepository _repo;

        public HocLieuCauHoiService(IHocLieuCauHoiRepository repo)
        {
            _repo = repo;
        }

        public async Task SaveHocLieuCauHoiAsync(HocLieuCauHoiRequest request)
        {
            await _repo.SaveAsync(request.HocLieuId, request.CauHoiIds);
        }

        public async Task<List<int>> GetSelectedCauHoiIdsAsync(int hocLieuId)
        {
            var data = await _repo.GetByHocLieuAsync(hocLieuId);
            return data.Select(x => x.CauHoiId).ToList();
        }
    }
}
