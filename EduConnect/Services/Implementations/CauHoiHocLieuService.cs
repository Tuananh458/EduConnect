using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<CauHoiHocLieuDto>> GetByHocLieuAsync(int hocLieuId)
        {
            var data = await _repo.GetByHocLieuAsync(hocLieuId);
            return data.Select(x => new CauHoiHocLieuDto
            {
                Id = x.Id,
                HocLieuId = x.HocLieuId,
                NoiDung = x.NoiDung,
                LoaiCauHoi = x.LoaiCauHoi,
                DoKho = x.DoKho,
                Diem = x.Diem,
                DapAnA = x.DapAnA,
                DapAnB = x.DapAnB,
                DapAnC = x.DapAnC,
                DapAnD = x.DapAnD,
                DapAnDung = x.DapAnDung
            }).ToList();
        }

        public async Task<CauHoiHocLieuDto?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return null;
            return new CauHoiHocLieuDto
            {
                Id = x.Id,
                HocLieuId = x.HocLieuId,
                NoiDung = x.NoiDung,
                LoaiCauHoi = x.LoaiCauHoi,
                DoKho = x.DoKho,
                Diem = x.Diem,
                DapAnA = x.DapAnA,
                DapAnB = x.DapAnB,
                DapAnC = x.DapAnC,
                DapAnD = x.DapAnD,
                DapAnDung = x.DapAnDung
            };
        }

        public async Task<CauHoiHocLieuDto> CreateAsync(CreateCauHoiHocLieuRequest request)
        {
            // đảm bảo học liệu tồn tại
            var hl = await _hocLieuRepo.GetByIdAsync(request.HocLieuId);
            if (hl == null)
                throw new System.Exception("Học liệu không tồn tại");

            var entity = new CauHoiHocLieu
            {
                HocLieuId = request.HocLieuId,
                NoiDung = request.NoiDung,
                LoaiCauHoi = request.LoaiCauHoi,
                DoKho = request.DoKho,
                Diem = request.Diem,
                DapAnA = request.DapAnA,
                DapAnB = request.DapAnB,
                DapAnC = request.DapAnC,
                DapAnD = request.DapAnD,
                DapAnDung = request.DapAnDung
            };

            var created = await _repo.AddAsync(entity);

            return new CauHoiHocLieuDto
            {
                Id = created.Id,
                HocLieuId = created.HocLieuId,
                NoiDung = created.NoiDung,
                LoaiCauHoi = created.LoaiCauHoi,
                DoKho = created.DoKho,
                Diem = created.Diem,
                DapAnA = created.DapAnA,
                DapAnB = created.DapAnB,
                DapAnC = created.DapAnC,
                DapAnD = created.DapAnD,
                DapAnDung = created.DapAnDung
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateCauHoiHocLieuRequest request)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.NoiDung = request.NoiDung;
            entity.LoaiCauHoi = request.LoaiCauHoi;
            entity.DoKho = request.DoKho;
            entity.Diem = request.Diem;
            entity.DapAnA = request.DapAnA;
            entity.DapAnB = request.DapAnB;
            entity.DapAnC = request.DapAnC;
            entity.DapAnD = request.DapAnD;
            entity.DapAnDung = request.DapAnDung;

            await _repo.UpdateAsync(entity);
            return true;
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
