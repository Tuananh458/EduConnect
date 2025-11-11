using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services.Implementations
{
    public class BaiLamHocLieuService : IBaiLamHocLieuService
    {
        private readonly AppDbContext _context;

        public BaiLamHocLieuService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeThiHocLieuDto?> GetDeThiAsync(int hocLieuId)
        {
            var hocLieu = await _context.HocLieus
                .Include(h => h.CauHois)
                .FirstOrDefaultAsync(h => h.HocLieuId == hocLieuId);

            if (hocLieu == null)
                return null;

            var cauHoiDtos = hocLieu.CauHois
                .Select((ch, index) => new CauHoiHocLieuDto
                {
                    Id = ch.Id,
                    ThuTu = index + 1,
                    NoiDung = ch.NoiDung,
                    DapAnA = ch.DapAnA,
                    DapAnB = ch.DapAnB,
                    DapAnC = ch.DapAnC,
                    DapAnD = ch.DapAnD,
                    DapAnDung = ch.DapAnDung,
                    LoaiCauHoi = ch.LoaiCauHoi
                })
                .ToList();

            return new DeThiHocLieuDto
            {
                HocLieuId = hocLieu.HocLieuId,
                TenHocLieu = hocLieu.TenHocLieu,
                CauHoi = cauHoiDtos
            };
        }

        public async Task<int> TaoBaiLamAsync(TaoBaiLamRequest dto)
        {
            var baiLam = new BaiLamHocLieu
            {
                HocLieuId = dto.HocLieuId,
                HocSinhId = dto.HocSinhId,
                NgayBatDau = DateTime.UtcNow,
                TrangThai = "Đang làm"
            };

            _context.BaiLamHocLieus.Add(baiLam);
            await _context.SaveChangesAsync();
            return baiLam.Id;
        }

        public async Task<bool> NopBaiAsync(NopBaiRequest dto)
        {
            var baiLam = await _context.BaiLamHocLieus
                .Include(x => x.ChiTiets)
                .FirstOrDefaultAsync(x => x.Id == dto.BaiLamId);

            if (baiLam == null)
            {
                baiLam = new BaiLamHocLieu
                {
                    HocLieuId = dto.HocLieuId,
                    HocSinhId = dto.HocSinhId,
                    NgayBatDau = DateTime.UtcNow,
                    TrangThai = "Đang làm"
                };
                _context.BaiLamHocLieus.Add(baiLam);
                await _context.SaveChangesAsync();
            }

            var old = _context.BaiLamChiTiets.Where(x => x.MaBaiLam == baiLam.Id);
            _context.BaiLamChiTiets.RemoveRange(old);

            double tongDiem = 0;
            double diemMoiCau = 10.0 / dto.CauTraLoi.Count;

            foreach (var cauTraLoi in dto.CauTraLoi)
            {
                var cauHoi = await _context.CauHoiHocLieus.FindAsync(cauTraLoi.CauHoiId);
                if (cauHoi == null) continue;

                bool dung = cauTraLoi.DapAnChon?.Trim()
                    .Equals(cauHoi.DapAnDung, StringComparison.OrdinalIgnoreCase) == true;
                if (dung) tongDiem += diemMoiCau;

                var chiTiet = new BaiLamChiTiet
                {
                    MaBaiLam = baiLam.Id,
                    MaCauHoi = cauHoi.Id,
                    TraLoi = cauTraLoi.DapAnChon,
                    DungSai = dung
                };
                _context.BaiLamChiTiets.Add(chiTiet);
            }

            baiLam.NgayNop = DateTime.UtcNow;
            baiLam.TrangThai = "Đã nộp";
            baiLam.Diem = Math.Round(tongDiem, 2);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<KetQuaBaiLamDto?> GetChiTietAsync(int baiLamId)
        {
            var baiLam = await _context.BaiLamHocLieus.FirstOrDefaultAsync(b => b.Id == baiLamId);
            if (baiLam == null) return null;

            var chiTiets = await _context.BaiLamChiTiets
                .Where(c => c.MaBaiLam == baiLamId)
                .ToListAsync();

            var ketQua = new KetQuaBaiLamDto
            {
                BaiLamId = baiLam.Id,
                HocLieuId = baiLam.HocLieuId,
                Diem = baiLam.Diem,
                ThoiGianNop = baiLam.NgayNop,
                CauTraLoi = new List<CauTraLoiKetQuaDto>()
            };

            foreach (var ct in chiTiets)
            {
                var cauHoi = await _context.CauHoiHocLieus.FindAsync(ct.MaCauHoi);
                if (cauHoi == null) continue;

                ketQua.CauTraLoi.Add(new CauTraLoiKetQuaDto
                {
                    CauHoiId = cauHoi.Id,
                    NoiDung = cauHoi.NoiDung,
                    DapAnDung = cauHoi.DapAnDung ?? "",
                    DapAnChon = ct.TraLoi ?? "",
                    Dung = ct.DungSai ?? false
                });
            }

            return ketQua;
        }

        public async Task<IEnumerable<BaiLamHocLieuDto>> GetKetQuaTheoHocLieuAsync(int hocLieuId)
        {
            return await _context.BaiLamHocLieus
                .Include(b => b.HocSinh)
                .ThenInclude(h => h.NguoiDung) // 🔁 đổi từ .User → .NguoiDung
                .Where(b => b.HocLieuId == hocLieuId && b.TrangThai == "Đã nộp")
                .Select(b => new BaiLamHocLieuDto
                {
                    BaiLamId = b.Id,
                    TenHocSinh = b.HocSinh.NguoiDung.HoTen, // 🔁 FullName → HoTen
                    Email = b.HocSinh.NguoiDung.Email,       // 🔁
                    Diem = b.Diem,
                    ThoiGianNop = b.NgayNop
                })
                .ToListAsync();
        }
    }
}
