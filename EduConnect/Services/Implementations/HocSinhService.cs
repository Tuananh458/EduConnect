using EduConnect.Repositories.Interfaces;
using EduConnect.Services.Interfaces;
using EduConnect.Models;
using EduConnect.Shared.DTOs.HocSinh;
using EduConnect.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EduConnect.Shared.DTOs.LopHoc;

namespace EduConnect.Services.Implementations
{
    public class HocSinhService : IHocSinhService
    {
        private readonly IHocSinhRepository _repo;
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<object> _hasher;

        public HocSinhService(IHocSinhRepository repo, AppDbContext context, IPasswordHasher<object> hasher)
        {
            _repo = repo;
            _context = context;
            _hasher = hasher;
        }

        public async Task<List<HocSinhDto>> GetByLopAsync(int maLopHoc)
        {
            var list = await _repo.GetByLopAsync(maLopHoc);
            return list.Select(h => new HocSinhDto
            {
                MaHocSinh = h.MaHocSinh,
                MaNguoiDung = h.MaNguoiDung,
                MaLopHoc = h.MaLopHoc,
                HoVaTen = h.NguoiDung?.HoTen ?? "",
                TenDangNhap = h.NguoiDung?.TenDangNhap ?? "",
                MaDinhDanh = h.MaDinhDanh,
                NgaySinh = h.NgaySinh,
                LaLopTruong = h.LaLopTruong,
                NgayTao = h.NgayTao
            }).ToList();
        }

        public async Task<bool> CreateAsync(CreateHocSinhRequest req)
        {
            if (await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == req.TenDangNhap))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");

            var nguoiDung = new NguoiDung
            {
                TenDangNhap = req.TenDangNhap,
                HoTen = req.HoVaTen,
                VaiTro = "HocSinh",
                Email = $"{req.TenDangNhap}@student.local",
                MatKhauHash = _hasher.HashPassword(null!, req.MatKhau),
                PhaiDoiMatKhau = true,
                TrangThai = 1
            };

            await _context.NguoiDungs.AddAsync(nguoiDung);
            await _context.SaveChangesAsync();

            var hocSinh = new HocSinh
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                MaLopHoc = req.MaLopHoc,
                MaDinhDanh = req.MaDinhDanh,
                NgaySinh = req.NgaySinh,
                NgayTao = DateTime.UtcNow
            };

            await _repo.AddAsync(hocSinh);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int maHocSinh)
        {
            await _repo.DeleteAsync(maHocSinh);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateHocSinhRequest req)
        {
            var hocSinh = await _context.HocSinhs.FindAsync(req.MaHocSinh);
            if (hocSinh == null) return false;

            var nguoiDung = await _context.NguoiDungs.FindAsync(hocSinh.MaNguoiDung);
            if (nguoiDung == null) return false;

            nguoiDung.HoTen = req.HoVaTen;
            nguoiDung.TenDangNhap = req.TenDangNhap;

            if (!string.IsNullOrWhiteSpace(req.MatKhau))
                nguoiDung.MatKhauHash = _hasher.HashPassword(null!, req.MatKhau);

            hocSinh.MaDinhDanh = req.MaDinhDanh;
            hocSinh.NgaySinh = req.NgaySinh;
            hocSinh.LaLopTruong = req.LaLopTruong;

            await _context.SaveChangesAsync();
            return true;
        }

        // 🚀 IMPORT DANH SÁCH HỌC SINH
        public async Task<ImportConfirmResult> ConfirmImportAsync(ImportConfirmRequest req)
        {
            var result = new ImportConfirmResult();

            foreach (var hs in req.Items)
            {
                try
                {
                    var existUser = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.TenDangNhap == hs.TenDangNhap);
                    var existHS = await _context.HocSinhs
                        .Include(h => h.NguoiDung)
                        .FirstOrDefaultAsync(h => h.MaDinhDanh == hs.MaDinhDanh || (h.NguoiDung != null && h.NguoiDung.TenDangNhap == hs.TenDangNhap));

                    if (existUser == null && existHS == null)
                    {
                        var nguoiDung = new NguoiDung
                        {
                            TenDangNhap = hs.TenDangNhap,
                            HoTen = hs.HoVaTen,
                            Email = $"{hs.TenDangNhap}@student.local",
                            VaiTro = "HocSinh",
                            TrangThai = 1,
                            PhaiDoiMatKhau = true,
                            MatKhauHash = _hasher.HashPassword(null!, hs.MatKhau)
                        };

                        await _context.NguoiDungs.AddAsync(nguoiDung);
                        await _context.SaveChangesAsync();

                        var newHS = new HocSinh
                        {
                            MaNguoiDung = nguoiDung.MaNguoiDung,
                            MaLopHoc = req.MaLopHoc,
                            MaDinhDanh = hs.MaDinhDanh,
                            NgaySinh = hs.NgaySinh,
                            NgayTao = DateTime.UtcNow
                        };

                        await _repo.AddAsync(newHS);
                        result.Created++;
                    }
                    else if (req.Action == "addToClass" && existHS != null)
                    {
                        existHS.MaLopHoc = req.MaLopHoc;
                        await _repo.UpdateAsync(existHS);
                        result.AddedToClass++;
                    }
                    else if (req.Action == "transferToClass" && existHS != null)
                    {
                        existHS.MaLopHoc = req.MaLopHoc;
                        await _repo.UpdateAsync(existHS);
                        result.Transferred++;
                    }
                    else
                    {
                        result.Skipped++;
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"[{hs.TenDangNhap}] {ex.Message}");
                }
            }

            await _repo.SaveChangesAsync();
            return result;
        }

        // 🚧 Tạo preview danh sách import
        public async Task<ImportPreviewResponse> GeneratePreviewAsync(IFormFile file, int maLopHoc, string? prefix = null, string? defaultPassword = null)
        {
            var response = new ImportPreviewResponse { MaLopHoc = maLopHoc, Prefix = prefix };

            if (file == null || file.Length == 0)
                throw new InvalidOperationException("File không hợp lệ.");

            var list = new List<HocSinhPreviewDto>();
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, $"{Guid.NewGuid()}_{file.FileName}");

            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext == ".csv")
            {
                using var reader = new StreamReader(path);
                string? line; int row = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    row++;
                    if (row == 1) continue;
                    var cols = line.Split(',', ';', '\t');
                    if (cols.Length < 5) continue;

                    list.Add(ParseRow(cols[0], cols[1], cols[2], cols[3], cols[4]));
                }
            }
            else
            {
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using var package = new OfficeOpenXml.ExcelPackage(new FileInfo(path));
                var ws = package.Workbook.Worksheets.FirstOrDefault();
                if (ws == null) throw new InvalidOperationException("Không tìm thấy sheet nào.");

                var rows = ws.Dimension.Rows;
                for (int i = 2; i <= rows; i++)
                {
                    list.Add(ParseRow(
                        ws.Cells[i, 1].Text?.Trim(),
                        ws.Cells[i, 2].Text?.Trim(),
                        ws.Cells[i, 3].Text?.Trim(),
                        ws.Cells[i, 4].Text?.Trim(),
                        ws.Cells[i, 5].Text?.Trim()
                    ));
                }
            }

            foreach (var x in list)
            {
                var errs = new List<string>();
                if (string.IsNullOrWhiteSpace(x.HoVaTen)) errs.Add("Thiếu họ tên");
                if (string.IsNullOrWhiteSpace(x.TenDangNhap)) errs.Add("Thiếu tên đăng nhập");
                else if (char.IsDigit(x.TenDangNhap[0])) errs.Add("Tên đăng nhập không được bắt đầu bằng số");
                if (string.IsNullOrWhiteSpace(x.MaDinhDanh)) errs.Add("Thiếu mã định danh");

                x.HopLe = errs.Count == 0;
                x.Loi = errs.Count == 0 ? null : string.Join(", ", errs);
            }

            response.Items = list;
            return response;

            HocSinhPreviewDto ParseRow(string? maDinhDanh, string? hoTen, string? ngaySinh, string? tenDangNhap, string? matKhau)
            {
                DateTime? ns = null;
                if (DateTime.TryParse(ngaySinh, out var d)) ns = d;

                var username = (tenDangNhap ?? "").Trim();
                if (!string.IsNullOrEmpty(prefix) && !username.StartsWith(prefix))
                    username = prefix + username;

                var password = !string.IsNullOrEmpty(matKhau)
                    ? matKhau
                    : (!string.IsNullOrEmpty(defaultPassword) ? defaultPassword : username);

                return new HocSinhPreviewDto
                {
                    MaDinhDanh = (maDinhDanh ?? "").Trim(),
                    HoVaTen = (hoTen ?? "").Trim(),
                    NgaySinh = ns,
                    TenDangNhap = username,
                    MatKhau = password
                };
            }
        }

        public async Task<int> DeleteManyAsync(List<int> ids)
        {
            await _repo.DeleteManyAsync(ids);
            return ids.Count;
        }

        public async Task<HocSinhDto?> GetByUserIdAsync(Guid maNguoiDung)
        {
            var hs = await _context.HocSinhs
                .Include(h => h.NguoiDung)
                .FirstOrDefaultAsync(h => h.MaNguoiDung == maNguoiDung);

            if (hs == null) return null;

            return new HocSinhDto
            {
                MaHocSinh = hs.MaHocSinh,
                HoVaTen = hs.NguoiDung.HoTen,
                MaLopHoc = hs.MaLopHoc
            };
        }
        public async Task<LopHocChiTietDto?> GetLopHocCuaToiAsync(Guid maNguoiDung)
        {
            // 🔹 1. Tìm học sinh theo mã người dùng
            var hocSinh = await _context.HocSinhs
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l.KhoiHoc)
                .Include(h => h.LopHoc)
                    .ThenInclude(l => l.GiaoVienChuNhiem)
                        .ThenInclude(gv => gv.NguoiDung)
                .FirstOrDefaultAsync(h => h.MaNguoiDung == maNguoiDung);

            if (hocSinh == null || hocSinh.LopHoc == null)
                return null;

            // 🔹 2. Lấy danh sách học sinh cùng lớp
            var dsCungLop = await _context.HocSinhs
                .Include(h => h.NguoiDung)
                .Where(h => h.MaLopHoc == hocSinh.MaLopHoc)
                .Select(h => new HocSinhTrongLopDto
                {
                    MaHocSinh = h.MaHocSinh,
                    HoVaTen = h.NguoiDung.HoTen,
                    LaLopTruong = h.LaLopTruong
                })
                .ToListAsync();

            // 🔹 3. Ghép dữ liệu chi tiết lớp học
            var lop = hocSinh.LopHoc;

            return new LopHocChiTietDto
            {
                MaLopHoc = lop.MaLopHoc,
                TenLopHoc = lop.TenLopHoc,
                TenKhoiHoc = lop.KhoiHoc?.TenKhoiHoc ?? "",
                SiSo = dsCungLop.Count,
                TenGiaoVienChuNhiem = lop.GiaoVienChuNhiem?.NguoiDung?.HoTen,
                EmailGiaoVien = lop.GiaoVienChuNhiem?.NguoiDung?.Email,
                AvatarGiaoVien = lop.GiaoVienChuNhiem?.NguoiDung?.AnhDaiDien,
                DanhSachHocSinh = dsCungLop
            };
        }
    }
}
