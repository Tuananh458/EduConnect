using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Implementations
{
    public class HocLieuRepository : IHocLieuRepository
    {
        private readonly AppDbContext _ctx;
        public HocLieuRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<HocLieu>> GetAllAsync(string? keyword = null, string? maLoai = null,
            string? nguonTao = null, bool? laTuDo = null, bool? laAn = null)
        {
            var query = _ctx.HocLieus.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.TenHocLieu.Contains(keyword));

            if (!string.IsNullOrWhiteSpace(maLoai) && maLoai != "all")
                query = query.Where(x => x.MaLoaiHocLieu == maLoai);

            if (!string.IsNullOrWhiteSpace(nguonTao) && nguonTao != "all")
                query = query.Where(x => x.NguonTao == nguonTao);

            if (laTuDo.HasValue)
                query = query.Where(x => x.LaHocLieuTuDo == laTuDo.Value);

            if (laAn.HasValue)
                query = query.Where(x => x.LaHocLieuAn == laAn.Value);

            return await query
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync();
        }

        public Task<HocLieu?> GetByIdAsync(int id)
        {
            return _ctx.HocLieus
                .Include(x => x.CauHois)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<HocLieu> AddAsync(HocLieu entity)
        {
            _ctx.HocLieus.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(HocLieu entity)
        {
            _ctx.HocLieus.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(HocLieu entity)
        {
            _ctx.HocLieus.Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
