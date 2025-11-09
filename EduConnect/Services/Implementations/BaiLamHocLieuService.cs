using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services.Implementations
{
    public class BaiLamHocLieuService : IBaiLamHocLieuService
    {
        private readonly AppDbContext _db;

        public BaiLamHocLieuService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<double> LuuBaiLamAsync(BaiLamHocLieuRequest req)
        {
            var cauHois = await _db.CauHoiHocLieus
                .Where(c => req.CauHoiDaLam.Select(x => x.CauHoiId).Contains(c.Id))
                .ToListAsync();

            double tongDiem = 0;
            var chiTiet = new List<ChiTietBaiLamHocLieu>();

            foreach (var lam in req.CauHoiDaLam)
            {
                var cauHoi = cauHois.FirstOrDefault(x => x.Id == lam.CauHoiId);
                if (cauHoi == null) continue;

                bool dung = lam.DapAnChon == cauHoi.DapAnDung;
                if (dung) tongDiem += cauHoi.Diem;

                chiTiet.Add(new ChiTietBaiLamHocLieu
                {
                    CauHoiId = cauHoi.Id,
                    DapAnChon = lam.DapAnChon,
                    DapAnDung = cauHoi.DapAnDung,
                    Diem = dung ? cauHoi.Diem : 0
                });
            }

            var baiLam = new BaiLamHocLieu
            {
                HocLieuId = req.HocLieuId,
                TenHocSinh = req.TenHocSinh,
                ThoiGianBatDau = DateTime.UtcNow,
                ThoiGianNop = DateTime.UtcNow,
                TongDiem = tongDiem,
                ChiTietBaiLams = chiTiet
            };

            _db.BaiLamHocLieus.Add(baiLam);
            await _db.SaveChangesAsync();

            return tongDiem;
        }
    }
}
