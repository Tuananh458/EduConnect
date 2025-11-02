using EduConnect.Models.HocLieu;
using EduConnect.Repositories.Interfaces;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Implementations
{
    public class CauHoiHocLieuService : ICauHoiHocLieuService
    {
        private readonly ICauHoiHocLieuRepository _repo;
        private readonly IHocLieuRepository _hocLieuRepo;

        public CauHoiHocLieuService(ICauHoiHocLieuRepository repo, IHocLieuRepository hocLieuRepo)
        {
            _repo = repo;
            _hocLieuRepo = hocLieuRepo;
        }

        public async Task<IEnumerable<CauHoiHocLieuDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(x => new CauHoiHocLieuDto
            {
                MaCauHoi = x.MaCauHoi,
                NoiDung = x.NoiDung,
                Loai = x.Loai,
                DoKho = x.DoKho,
                GiaiThich = x.GiaiThich,
                NgayTao = x.NgayTao
            });
        }

        public async Task<IEnumerable<CauHoiHocLieuDto>> GetByHocLieuAsync(int maHocLieu)
        {
            var data = await _repo.GetByHocLieuAsync(maHocLieu);
            return data.Select(x => new CauHoiHocLieuDto
            {
                MaCauHoi = x.MaCauHoi,
                NoiDung = x.NoiDung,
                Loai = x.Loai,
                DoKho = x.DoKho,
                GiaiThich = x.GiaiThich,
                NgayTao = x.NgayTao
            });
        }

        public async Task<int> CreateAsync(CreateCauHoiHocLieuRequest request)
        {
            var entity = new CauHoiHocLieu
            {
                NoiDung = request.NoiDung,
                Loai = request.Loai,
                DoKho = request.DoKho,
                GiaiThich = request.GiaiThich,
                NgayTao = DateTime.Now
            };

            await _repo.AddAsync(entity);

            // Nếu tạo trong học liệu thì gán luôn
            if (request.MaHocLieu.HasValue)
            {
                var hocLieu = await _hocLieuRepo.GetByIdAsync(request.MaHocLieu.Value);
                if (hocLieu != null)
                {
                    hocLieu.HocLieuCauHois.Add(new HocLieuCauHoi
                    {
                        MaHocLieu = hocLieu.MaHocLieu,
                        MaCauHoi = entity.MaCauHoi,
                        Diem = 1
                    });
                    await _hocLieuRepo.UpdateAsync(hocLieu);
                }
            }

            return entity.MaCauHoi;
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
