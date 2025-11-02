using EduConnect.Data;
using EduConnect.Models.HocLieu;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CauHoiHocLieuController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CauHoiHocLieuController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/CauHoiHocLieu
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.CauHoiHocLieus
                .OrderByDescending(x => x.NgayTao)
                .Select(x => new CauHoiHocLieuDto
                {
                    MaCauHoi = x.MaCauHoi,
                    NoiDung = x.NoiDung,
                    Loai = x.Loai,
                    DoKho = x.DoKho,
                    GiaiThich = x.GiaiThich,
                    NgayTao = x.NgayTao
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET api/CauHoiHocLieu/byHocLieu/5
        [HttpGet("byHocLieu/{maHocLieu:int}")]
        public async Task<IActionResult> GetByHocLieu(int maHocLieu)
        {
            var data = await _context.HocLieuCauHois
                .Include(x => x.CauHoiHocLieu)
                .Where(x => x.MaHocLieu == maHocLieu)
                .Select(x => new CauHoiHocLieuDto
                {
                    MaCauHoi = x.CauHoiHocLieu!.MaCauHoi,
                    NoiDung = x.CauHoiHocLieu.NoiDung,
                    Loai = x.CauHoiHocLieu.Loai,
                    DoKho = x.CauHoiHocLieu.DoKho,
                    GiaiThich = x.CauHoiHocLieu.GiaiThich,
                    NgayTao = x.CauHoiHocLieu.NgayTao
                })
                .ToListAsync();

            return Ok(data);
        }

        // POST api/CauHoiHocLieu
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCauHoiHocLieuRequest req)
        {
            var entity = new CauHoiHocLieu
            {
                NoiDung = req.NoiDung,
                Loai = req.Loai,
                DoKho = req.DoKho,
                GiaiThich = req.GiaiThich,
                NgayTao = DateTime.Now
            };

            _context.CauHoiHocLieus.Add(entity);
            await _context.SaveChangesAsync();

            // nếu tạo ngay trong một học liệu thì tự gán luôn
            if (req.MaHocLieu.HasValue)
            {
                _context.HocLieuCauHois.Add(new HocLieuCauHoi
                {
                    MaHocLieu = req.MaHocLieu.Value,
                    MaCauHoi = entity.MaCauHoi,
                    Diem = 1
                });

                await _context.SaveChangesAsync();
            }

            return Ok(new { maCauHoi = entity.MaCauHoi, message = "Tạo câu hỏi thành công" });
        }
    }
}
