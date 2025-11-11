using EduConnect.Data;
using EduConnect.Models;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduConnect.Repositories.Implementations
{
    public class LopHocRepository : ILopHocRepository
    {
        private readonly AppDbContext _context;

        public LopHocRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LopHoc>> GetAllAsync()
        {
            return await _context.Set<LopHoc>()
                                 .Include(x => x.KhoiHoc)
                                 .OrderByDescending(x => x.NgayTao)
                                 .ToListAsync()
                                 .ConfigureAwait(false);
        }

        public async Task<IEnumerable<LopHoc>> GetAllByNguoiTaoAsync(Guid nguoiTaoId)
        {
            return await _context.Set<LopHoc>()
                                 .Include(x => x.KhoiHoc)
                                 .Where(x => x.NguoiTaoId == nguoiTaoId)
                                 .OrderByDescending(x => x.NgayTao)
                                 .ToListAsync()
                                 .ConfigureAwait(false);
        }


        public async Task<LopHoc?> GetByIdAsync(int id)
        {
            return await _context.Set<LopHoc>()
                                 .Include(x => x.KhoiHoc)
                                 .FirstOrDefaultAsync(x => x.MaLopHoc == id)
                                 .ConfigureAwait(false);
        }

        public async Task AddAsync(LopHoc entity)
        {
            _context.Set<LopHoc>().Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(LopHoc entity)
        {
            _context.Set<LopHoc>().Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(LopHoc entity)
        {
            _context.Set<LopHoc>().Remove(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<LopHoc>()
                                 .AnyAsync(x => x.MaLopHoc == id)
                                 .ConfigureAwait(false);
        }
        public async Task<bool> AnyAsync(int maKhoiHoc, string tenLopHoc)
        {
            return await _context.Set<LopHoc>()
                                 .AnyAsync(x => x.MaKhoiHoc == maKhoiHoc && x.TenLopHoc == tenLopHoc)
                                 .ConfigureAwait(false);
        }

    }
}
