using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services.Implementations
{
    public class HocLieuService : IHocLieuService
    {
        private readonly IHocLieuRepository _repo;
        private readonly AppDbContext _db;

        public HocLieuService(IHocLieuRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        // 📘 Lấy danh sách học liệu có lọc
        public async Task<List<HocLieuListDto>> GetAllAsync(HocLieuFilterDto filter)
        {
            var data = await _repo.GetAllAsync(
                filter.Keyword,
                filter.MaLoaiHocLieu,
                filter.NguonTao,
                filter.LaHocLieuTuDo ? true : (bool?)null,
                filter.LaHocLieuAn ? true : (bool?)null
            );

            return data.Select(x => new HocLieuListDto
            {
                Id = x.Id,
                TenHocLieu = x.TenHocLieu,
                NgayTao = x.NgayTao,
                TheLoai = x.TenLoaiHocLieu ?? x.MaLoaiHocLieu,
                TenKhoaHoc = x.TenKhoaHoc
            }).ToList();
        }

        // 📘 Lấy chi tiết học liệu
        public async Task<HocLieuDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            return new HocLieuDto
            {
                Id = entity.Id,
                TenHocLieu = entity.TenHocLieu,
                MaLoaiHocLieu = entity.MaLoaiHocLieu,
                TenLoaiHocLieu = entity.TenLoaiHocLieu,
                NgayTao = entity.NgayTao,
                NguonTao = entity.NguonTao,
                LaHocLieuAn = entity.LaHocLieuAn,
                LaHocLieuTuDo = entity.LaHocLieuTuDo,
                TenKhoaHoc = entity.TenKhoaHoc
            };
        }

        // 📘 Tạo mới học liệu
        public async Task<HocLieuDto> CreateAsync(CreateHocLieuRequest request)
        {
            var entity = new HocLieu
            {
                TenHocLieu = request.TenHocLieu,
                MaLoaiHocLieu = request.MaLoaiHocLieu,
                TenLoaiHocLieu = request.TenLoaiHocLieu,
                NguonTao = request.NguonTao,
                LaHocLieuTuDo = request.LaHocLieuTuDo,
                LaHocLieuAn = request.LaHocLieuAn
            };

            var created = await _repo.AddAsync(entity);

            return new HocLieuDto
            {
                Id = created.Id,
                TenHocLieu = created.TenHocLieu,
                MaLoaiHocLieu = created.MaLoaiHocLieu,
                TenLoaiHocLieu = created.TenLoaiHocLieu,
                NguonTao = created.NguonTao,
                LaHocLieuTuDo = created.LaHocLieuTuDo,
                LaHocLieuAn = created.LaHocLieuAn,
                NgayTao = created.NgayTao
            };
        }

        // 📘 Xóa học liệu
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            return true;
        }

        // ✅ Bổ sung: Lấy danh sách câu hỏi trong học liệu
        public async Task<List<CauHoiHocLieuDto>> GetCauHoiTrongHocLieuAsync(int hocLieuId)
        {
            var cauHoiIds = await _db.HocLieuCauHois
                .Where(x => x.HocLieuId == hocLieuId)
                .OrderBy(x => x.ThuTu)
                .Select(x => x.CauHoiId)
                .ToListAsync();

            if (!cauHoiIds.Any()) return new List<CauHoiHocLieuDto>();

            var cauHois = await _db.CauHoiHocLieus
                .Where(x => cauHoiIds.Contains(x.Id))
                .Select(x => new CauHoiHocLieuDto
                {
                    Id = x.Id,
                    NoiDung = x.NoiDung,
                    DapAnA = x.DapAnA,
                    DapAnB = x.DapAnB,
                    DapAnC = x.DapAnC,
                    DapAnD = x.DapAnD,
                    DapAnDung = x.DapAnDung,
                    Diem = x.Diem
                })
                .ToListAsync();

            return cauHois;
        }

    }
}
