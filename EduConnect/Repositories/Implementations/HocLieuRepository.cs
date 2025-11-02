using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Implementations
{
    public class HocLieuRepository : IHocLieuRepository
    {
        private readonly AppDbContext _context;
        public HocLieuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HocLieu>> GetAllAsync()
        {
            return await _context.HocLieus
                .OrderByDescending(x => x.NgayTao)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<HocLieu?> GetByIdAsync(int id)
        {
            return await _context.HocLieus
                .Include(x => x.HocLieuCauHois)
                .FirstOrDefaultAsync(x => x.MaHocLieu == id);
        }

        public async Task AddAsync(HocLieu entity)
        {
            _context.HocLieus.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HocLieu entity)
        {
            _context.HocLieus.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(HocLieu entity)
        {
            _context.HocLieus.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
