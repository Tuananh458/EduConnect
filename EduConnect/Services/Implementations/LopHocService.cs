using EduConnect.Models;
using EduConnect.Repositories.Interfaces;
using EduConnect.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using EduConnect.Shared.DTOs.LopHoc;


namespace EduConnect.Services.Implementations
{
    public class LopHocService : ILopHocService
    {
        private readonly ILopHocRepository _repo;

        public LopHocService(ILopHocRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<LopHocDto>> GetAllByNguoiTaoAsync(Guid nguoiTaoId)
        {
            var data = await _repo.GetAllByNguoiTaoAsync(nguoiTaoId).ConfigureAwait(false);

            return data.Select(x => new LopHocDto
            {
                MaLopHoc = x.MaLopHoc,
                TenLopHoc = x.TenLopHoc,
                MaKhoiHoc = x.MaKhoiHoc,
                SiSo = x.SiSo,
                TrangThai = x.TrangThai,
                NgayTao = x.NgayTao,
                TenKhoiHoc = x.KhoiHoc?.TenKhoiHoc,
                NguoiTaoId = x.NguoiTaoId
            });
        }


        public async Task<IEnumerable<LopHocDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync().ConfigureAwait(false);
            return data.Select(x => new LopHocDto
            {
                MaLopHoc = x.MaLopHoc,
                TenLopHoc = x.TenLopHoc,
                MaKhoiHoc = x.MaKhoiHoc,
                SiSo = x.SiSo,
                TrangThai = x.TrangThai,
                NgayTao = x.NgayTao,
                TenKhoiHoc = x.KhoiHoc?.TenKhoiHoc
            });
        }

        public async Task<LopHocDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id).ConfigureAwait(false);
            if (entity == null) return null;

            return new LopHocDto
            {
                MaLopHoc = entity.MaLopHoc,
                TenLopHoc = entity.TenLopHoc,
                MaKhoiHoc = entity.MaKhoiHoc,
                SiSo = entity.SiSo,
                TrangThai = entity.TrangThai,
                NgayTao = entity.NgayTao,
                TenKhoiHoc = entity.KhoiHoc?.TenKhoiHoc
            };
        }

        public async Task CreateAsync(CreateLopHocRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var tenLops = request.TenLopHoc
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var ten in tenLops)
            {
                var existed = await _repo.AnyAsync(request.MaKhoiHoc, ten);
                if (existed)
                    throw new InvalidOperationException($"Lớp {ten} đã tồn tại trong khối này!");

                var newClass = new LopHoc
                {
                    TenLopHoc = ten,
                    MaKhoiHoc = request.MaKhoiHoc,
                    SiSo = 0,
                    TrangThai = "Hoạt động",
                    NgayTao = DateTime.Now,
                    NguoiTaoId = request.NguoiTaoId   // ✅ Gán người tạo
                };

                await _repo.AddAsync(newClass).ConfigureAwait(false);
            }
        }

        public async Task UpdateAsync(UpdateLopHocRequest request)
        {
            var entity = await _repo.GetByIdAsync(request.MaLopHoc).ConfigureAwait(false);
            if (entity == null)
                throw new KeyNotFoundException("Không tìm thấy lớp học để cập nhật");

            // ✅ Kiểm tra quyền: chỉ người tạo được phép sửa
            if (request.NguoiTaoId != entity.NguoiTaoId)
                throw new UnauthorizedAccessException("Bạn không có quyền chỉnh sửa lớp này.");

            entity.TenLopHoc = request.TenLopHoc;
            entity.MaKhoiHoc = request.MaKhoiHoc;
            entity.SiSo = request.SiSo;
            entity.TrangThai = request.TrangThai;

            await _repo.UpdateAsync(entity).ConfigureAwait(false);
        }


        public async Task DeleteAsync(int id, Guid nguoiTaoId)
        {
            var entity = await _repo.GetByIdAsync(id).ConfigureAwait(false);
            if (entity == null)
                throw new KeyNotFoundException("Không tìm thấy lớp học để xóa");

            // ✅ Kiểm tra quyền
            if (entity.NguoiTaoId != nguoiTaoId)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa lớp này.");

            await _repo.DeleteAsync(entity).ConfigureAwait(false);
        }

    }

}
