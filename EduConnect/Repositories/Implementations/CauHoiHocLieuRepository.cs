using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Implementations
{
    public class CauHoiHocLieuRepository : ICauHoiHocLieuRepository
    {
        private readonly AppDbContext _ctx;

        public CauHoiHocLieuRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<CauHoiHocLieu>> GetByHocLieuAsync(int hocLieuId)
        {
            return await _ctx.CauHoiHocLieus
                .Where(x => x.HocLieuId == hocLieuId)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public Task<CauHoiHocLieu?> GetByIdAsync(int id)
        {
            return _ctx.CauHoiHocLieus.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CauHoiHocLieu> AddAsync(CauHoiHocLieu entity)
        {
            _ctx.CauHoiHocLieus.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(CauHoiHocLieu entity)
        {
            _ctx.CauHoiHocLieus.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(CauHoiHocLieu entity)
        {
            _ctx.CauHoiHocLieus.Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
