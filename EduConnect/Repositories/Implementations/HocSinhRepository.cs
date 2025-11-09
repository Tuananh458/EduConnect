using EduConnect.Data;
using EduConnect.Models;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Implementations
{
    public class HocSinhRepository : IHocSinhRepository
    {
        private readonly AppDbContext _context;
        public HocSinhRepository(AppDbContext context) => _context = context;

        public async Task<List<HocSinh>> GetByLopAsync(int maLopHoc)
        {
            return await _context.HocSinhs
                .Include(h => h.User)
                .Where(h => h.MaLopHoc == maLopHoc)
                .ToListAsync();
        }

        public async Task<HocSinh?> GetByIdAsync(int maHocSinh)
        {
            return await _context.HocSinhs
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.MaHocSinh == maHocSinh);
        }

        public async Task AddAsync(HocSinh hocSinh) =>
            await _context.HocSinhs.AddAsync(hocSinh);

        public async Task DeleteAsync(int maHocSinh)
        {
            var hs = await _context.HocSinhs.FindAsync(maHocSinh);
            if (hs != null) _context.HocSinhs.Remove(hs);
        }
        public async Task UpdateAsync(HocSinh hocSinh)
        {
            _context.HocSinhs.Update(hocSinh);
            await Task.CompletedTask; 
        }
        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
        public async Task DeleteManyAsync(List<int> ids)
        {
            var list = await _context.HocSinhs.Where(x => ids.Contains(x.MaHocSinh)).ToListAsync();
            if (list.Any())
            {
                _context.HocSinhs.RemoveRange(list);
                await _context.SaveChangesAsync();
            }
        }

    }
}
