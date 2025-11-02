using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Implementations
{
    public class CauHoiHocLieuRepository : ICauHoiHocLieuRepository
    {
        private readonly AppDbContext _context;
        public CauHoiHocLieuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CauHoiHocLieu>> GetAllAsync()
        {
            return await _context.CauHoiHocLieus
                .OrderByDescending(x => x.NgayTao)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<CauHoiHocLieu>> GetByHocLieuAsync(int maHocLieu)
        {
            return await _context.HocLieuCauHois
                .Include(x => x.CauHoiHocLieu)
                .Where(x => x.MaHocLieu == maHocLieu)
                .Select(x => x.CauHoiHocLieu!)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CauHoiHocLieu?> GetByIdAsync(int id)
        {
            return await _context.CauHoiHocLieus
                .FirstOrDefaultAsync(x => x.MaCauHoi == id);
        }

        public async Task AddAsync(CauHoiHocLieu entity)
        {
            _context.CauHoiHocLieus.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CauHoiHocLieu entity)
        {
            _context.CauHoiHocLieus.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
