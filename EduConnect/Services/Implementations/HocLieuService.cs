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
            var query = _db.HocLieus.AsQueryable();

            // ✅ Lọc theo người tạo
            if (filter.NguoiTaoId != Guid.Empty)
                query = query.Where(h => h.NguoiTaoId == filter.NguoiTaoId);

            // ✅ Áp dụng các filter khác nếu có
            if (!string.IsNullOrEmpty(filter.Keyword))
                query = query.Where(x => x.TenHocLieu.Contains(filter.Keyword));

            if (filter.MaLoaiHocLieu != "all")
                query = query.Where(x => x.MaLoaiHocLieu == filter.MaLoaiHocLieu);

            if (filter.NguonTao != "all")
                query = query.Where(x => x.NguonTao == filter.NguonTao);

            if (filter.LaHocLieuTuDo)
                query = query.Where(x => x.LaHocLieuTuDo);

            if (filter.LaHocLieuAn)
                query = query.Where(x => x.LaHocLieuAn);

            var data = await query
                .OrderByDescending(x => x.NgayTao)
                .Select(x => new HocLieuListDto
                {
                    Id = x.HocLieuId,
                    TenHocLieu = x.TenHocLieu,
                    NgayTao = x.NgayTao,
                    TheLoai = x.TenLoaiHocLieu ?? x.MaLoaiHocLieu,
                    TenKhoaHoc = x.TenKhoaHoc
                })
                .ToListAsync();

            return data;
        }


        // 📘 Lấy chi tiết học liệu
        // 📘 Lấy chi tiết học liệu (kèm câu hỏi tự tạo và câu hỏi được chọn)
        public async Task<HocLieuDto?> GetByIdAsync(int id)
{
    var hocLieu = await _db.HocLieus
        .Include(h => h.CauHois) // 🟢 câu hỏi tự tạo trong học liệu
        .Include(h => h.HocLieuCauHois) // 🟢 câu hỏi được chọn từ ngân hàng
            .ThenInclude(hlch => hlch.CauHoi)
        .FirstOrDefaultAsync(h => h.HocLieuId == id);

    if (hocLieu == null)
        return null;

    var dto = new HocLieuDto
    {
        Id = hocLieu.HocLieuId,
        TenHocLieu = hocLieu.TenHocLieu,
        MaLoaiHocLieu = hocLieu.MaLoaiHocLieu,
        TenLoaiHocLieu = hocLieu.TenLoaiHocLieu,
        NguonTao = hocLieu.NguonTao,
        LaHocLieuAn = hocLieu.LaHocLieuAn,
        LaHocLieuTuDo = hocLieu.LaHocLieuTuDo,
        NgayTao = hocLieu.NgayTao,
        TenKhoaHoc = hocLieu.TenKhoaHoc,

        // ✅ Câu hỏi tự tạo
        CauHoiTuTao = hocLieu.CauHois.Select(ch => new CauHoiHocLieuDto
        {
            Id = ch.Id,
            HocLieuId = ch.HocLieuId,
            TieuDe = ch.TieuDe,
            NoiDung = ch.NoiDung,
            LoaiCauHoi = ch.LoaiCauHoi,
            DoKho = ch.DoKho,
            Diem = ch.Diem,
            DapAnA = ch.DapAnA,
            DapAnB = ch.DapAnB,
            DapAnC = ch.DapAnC,
            DapAnD = ch.DapAnD,
            DapAnDung = ch.DapAnDung
        }).ToList(),

        // ✅ Câu hỏi được chọn từ ngân hàng (qua bảng HocLieuCauHoi)
        CauHoiDuocChon = hocLieu.HocLieuCauHois.Select(x => new CauHoiNganHangDto
        {
            Id = x.CauHoi.Id,
            TieuDe = x.CauHoi.TieuDe,
            NoiDung = x.CauHoi.NoiDung,
            LoaiCauHoi = x.CauHoi.LoaiCauHoi,
            DoKho = x.CauHoi.DoKho,
            Diem = x.CauHoi.Diem,
            DapAnA = x.CauHoi.DapAnA,
            DapAnB = x.CauHoi.DapAnB,
            DapAnC = x.CauHoi.DapAnC,
            DapAnD = x.CauHoi.DapAnD,
            DapAnDung = x.CauHoi.DapAnDung,
            ThuTu = x.ThuTu
        }).ToList()
    };

    return dto;
}



        // 📘 Tạo mới học liệu
        public async Task<HocLieuDto> CreateAsync(CreateHocLieuRequest request)
        {
            var entity = new HocLieu
            {
                TenHocLieu = request.TenHocLieu,
                MaLoaiHocLieu = request.MaLoaiHocLieu,
                TenLoaiHocLieu = request.TenLoaiHocLieu,
                NguonTao = request.NguonTao ?? "Tự tạo",
                LaHocLieuTuDo = request.LaHocLieuTuDo,
                LaHocLieuAn = request.LaHocLieuAn,
                NguoiTaoId = request.NguoiTaoId 
            };

            var created = await _repo.AddAsync(entity);


            return new HocLieuDto
            {
                Id = created.HocLieuId,
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
