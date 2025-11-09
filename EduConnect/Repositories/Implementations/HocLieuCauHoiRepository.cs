using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Repositories.Implementations
{
    public class HocLieuCauHoiRepository : IHocLieuCauHoiRepository
    {
        private readonly AppDbContext _context;

        public HocLieuCauHoiRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HocLieuCauHoi>> GetByHocLieuAsync(int hocLieuId)
        {
            return await _context.HocLieuCauHois
                .Where(x => x.HocLieuId == hocLieuId)
                .OrderBy(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task SaveAsync(int hocLieuId, List<int> cauHoiIds)
        {
            var existing = _context.HocLieuCauHois.Where(x => x.HocLieuId == hocLieuId);
            _context.HocLieuCauHois.RemoveRange(existing);

            int index = 1;
            foreach (var id in cauHoiIds)
            {
                _context.HocLieuCauHois.Add(new HocLieuCauHoi
                {
                    HocLieuId = hocLieuId,
                    CauHoiId = id,
                    ThuTu = index++
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
