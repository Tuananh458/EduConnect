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

        public async Task<IEnumerable<HocLieuDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(x => new HocLieuDto
            {
                MaHocLieu = x.MaHocLieu,
                TenHocLieu = x.TenHocLieu,
                TheLoai = x.TheLoai,
                DaDuyet = x.DaDuyet,
                HienThi = x.HienThi,
                NgayTao = x.NgayTao
            });
        }

        public async Task<HocLieuDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            return new HocLieuDto
            {
                MaHocLieu = entity.MaHocLieu,
                TenHocLieu = entity.TenHocLieu,
                TheLoai = entity.TheLoai,
                DaDuyet = entity.DaDuyet,
                HienThi = entity.HienThi,
                NgayTao = entity.NgayTao
            };
        }

        public async Task<int> CreateAsync(CreateHocLieuRequest request)
        {
            var entity = new HocLieu
            {
                TenHocLieu = request.TenHocLieu,
                TheLoai = request.TheLoai,
                DaDuyet = false,
                HienThi = true,
                NgayTao = DateTime.Now
            };

            await _repo.AddAsync(entity);
            return entity.MaHocLieu;
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
