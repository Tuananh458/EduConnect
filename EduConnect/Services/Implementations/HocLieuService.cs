using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Implementations
{
    public class HocLieuService : IHocLieuService
    {
        private readonly IHocLieuRepository _repo;

        public HocLieuService(IHocLieuRepository repo)
        {
            _repo = repo;
        }

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

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            return true;
        }
    }
}
