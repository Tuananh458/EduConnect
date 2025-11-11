using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Implementations
{
    public class BaiLamHocLieuRepository : IBaiLamHocLieuRepository
    {
        private readonly AppDbContext _context;

        public BaiLamHocLieuRepository(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 Lấy danh sách tất cả bài làm (cho giáo viên quản lý)
        public async Task<IEnumerable<BaiLamHocLieu>> GetAllAsync()
        {
            return await _context.BaiLamHocLieus
                .Include(b => b.HocSinh)
                .Include(b => b.HocLieu)
                .Include(b => b.ChiTiets)
                .ToListAsync();
        }

        // 🟢 Lấy danh sách bài làm theo học liệu (ví dụ: bài thi của 1 học liệu cụ thể)
        public async Task<IEnumerable<BaiLamHocLieu>> GetByHocLieuAsync(int hocLieuId)
        {
            return await _context.BaiLamHocLieus
                .Include(b => b.HocSinh)
                .Include(b => b.ChiTiets)
                .Where(b => b.HocLieuId == hocLieuId)
                .ToListAsync();
        }

        // 🟢 Lấy bài làm theo học sinh (một học sinh làm nhiều học liệu)
        public async Task<IEnumerable<BaiLamHocLieu>> GetByHocSinhAsync(int hocSinhId)
        {
            return await _context.BaiLamHocLieus
                .Include(b => b.HocLieu)
                .Include(b => b.ChiTiets)
                .Where(b => b.HocSinhId == hocSinhId)
                .ToListAsync();
        }

        // 🟢 Lấy chi tiết 1 bài làm theo ID
        public async Task<BaiLamHocLieu?> GetByIdAsync(int baiLamId)
        {
            return await _context.BaiLamHocLieus
                .Include(b => b.HocSinh)
                .Include(b => b.HocLieu)
                .Include(b => b.ChiTiets)
                .ThenInclude(ct => ct.BaiLamHocLieu)
                .FirstOrDefaultAsync(b => b.Id == baiLamId);
        }

        // 🟢 Thêm mới bài làm
        public async Task<BaiLamHocLieu> AddAsync(BaiLamHocLieu baiLam)
        {
            _context.BaiLamHocLieus.Add(baiLam);
            await _context.SaveChangesAsync();
            return baiLam;
        }

        // 🟢 Cập nhật bài làm
        public async Task UpdateAsync(BaiLamHocLieu baiLam)
        {
            _context.BaiLamHocLieus.Update(baiLam);
            await _context.SaveChangesAsync();
        }

        // 🟢 Xóa bài làm (nếu cần)
        public async Task DeleteAsync(int baiLamId)
        {
            var baiLam = await _context.BaiLamHocLieus.FindAsync(baiLamId);
            if (baiLam != null)
            {
                _context.BaiLamHocLieus.Remove(baiLam);
                await _context.SaveChangesAsync();
            }
        }

        // 🟢 Kiểm tra xem học sinh đã làm bài chưa
        public async Task<BaiLamHocLieu?> GetByHocSinhVaHocLieuAsync(int hocSinhId, int hocLieuId)
        {
            return await _context.BaiLamHocLieus
                .Include(b => b.ChiTiets)
                .FirstOrDefaultAsync(b => b.HocSinhId == hocSinhId && b.HocLieuId == hocLieuId);
        }
    }
}
