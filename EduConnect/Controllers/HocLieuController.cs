using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HocLieuController : ControllerBase
    {
        private readonly AppDbContext _context;
        public HocLieuController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.HocLieus
                .OrderByDescending(x => x.NgayTao)
                .Select(x => new HocLieuDto
                {
                    MaHocLieu = x.MaHocLieu,
                    TenHocLieu = x.TenHocLieu,
                    TheLoai = x.TheLoai,
                    DaDuyet = x.DaDuyet,
                    HienThi = x.HienThi,
                    NgayTao = x.NgayTao
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var h = await _context.HocLieus.FindAsync(id);
            if (h == null) return NotFound();

            var dto = new HocLieuDto
            {
                MaHocLieu = h.MaHocLieu,
                TenHocLieu = h.TenHocLieu,
                TheLoai = h.TheLoai,
                DaDuyet = h.DaDuyet,
                HienThi = h.HienThi,
                NgayTao = h.NgayTao
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHocLieuRequest req)
        {
            var entity = new HocLieu
            {
                TenHocLieu = req.TenHocLieu,
                TheLoai = req.TheLoai,
                HienThi = true,
                DaDuyet = false,
                NgayTao = DateTime.Now,
                NguoiTao = User?.Identity?.Name ?? "system"
            };

            _context.HocLieus.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new { maHocLieu = entity.MaHocLieu, message = "Thêm học liệu thành công" });
        }
    }
}
