using EduConnect.Repositories.Interfaces;
using EduConnect.Services.Interfaces;
using EduConnect.Models;
using EduConnect.Shared.DTOs.HocSinh;
using EduConnect.Data;
using EduConnect.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                UserId = h.UserId,
                MaLopHoc = h.MaLopHoc,
                HoVaTen = h.User?.FullName ?? "",
                TenDangNhap = h.User?.Username ?? "",
                MaDinhDanh = h.MaDinhDanh,
                NgaySinh = h.NgaySinh,
                LaLopTruong = h.LaLopTruong,
                NgayTao = h.NgayTao
            }).ToList();
        }

        public async Task<bool> CreateAsync(CreateHocSinhRequest req)
        {
            // 🔹 Kiểm tra trùng username trước
            if (await _context.Users.AnyAsync(u => u.Username == req.TenDangNhap))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");

            var user = new User
            {
                Username = req.TenDangNhap,
                FullName = req.HoVaTen,
                Role = "Student",
                Email = $"{req.TenDangNhap}@student.local", // tránh lỗi NOT NULL
                PasswordHash = _hasher.HashPassword(null!, req.MatKhau),
                MustChangePassword = true,
                Status = 1
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var hocSinh = new HocSinh
            {
                UserId = user.UserId,
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

            var user = await _context.Users.FindAsync(hocSinh.UserId);
            if (user == null) return false;

            user.FullName = req.HoVaTen;
            user.Username = req.TenDangNhap;

            if (!string.IsNullOrWhiteSpace(req.MatKhau))
                user.PasswordHash = _hasher.HashPassword(null!, req.MatKhau);

            hocSinh.MaDinhDanh = req.MaDinhDanh;
            hocSinh.NgaySinh = req.NgaySinh;
            hocSinh.LaLopTruong = req.LaLopTruong;

            await _context.SaveChangesAsync();
            return true;
        }
        // ==============================
        // 🚀 IMPORT DANH SÁCH HỌC SINH
        // ==============================

        public async Task<ImportConfirmResult> ConfirmImportAsync(ImportConfirmRequest req)
        {
            var result = new ImportConfirmResult();

            foreach (var hs in req.Items)
            {
                try
                {
                    // kiểm tra User hoặc HocSinh đã tồn tại
                    var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == hs.TenDangNhap);
                    var existHS = await _context.HocSinhs
                        .Include(h => h.User)
                        .FirstOrDefaultAsync(h => h.MaDinhDanh == hs.MaDinhDanh || (h.User != null && h.User.Username == hs.TenDangNhap));

                    if (existUser == null && existHS == null)
                    {
                        // tạo mới tài khoản học sinh
                        var user = new User
                        {
                            Username = hs.TenDangNhap,
                            FullName = hs.HoVaTen,
                            Email = $"{hs.TenDangNhap}@student.local",
                            Role = "Student",
                            Status = 1,
                            MustChangePassword = true,
                            PasswordHash = _hasher.HashPassword(null!, hs.MatKhau)
                        };

                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();

                        var newHS = new HocSinh
                        {
                            UserId = user.UserId,
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
                        // thêm học sinh đã có vào lớp hiện tại
                        existHS.MaLopHoc = req.MaLopHoc;
                        await _repo.UpdateAsync(existHS);
                        result.AddedToClass++;
                    }
                    else if (req.Action == "transferToClass" && existHS != null)
                    {
                        // chuyển lớp
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

        // =======================================
        // 🚧 (Tùy chọn) Sinh trước danh sách demo
        // =======================================
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
                    if (row == 1) continue; // bỏ header
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

            // Kiểm tra hợp lệ
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



    }
}
