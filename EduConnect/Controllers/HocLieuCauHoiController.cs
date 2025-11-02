using EduConnect.Data;
using EduConnect.Models.HocLieu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HocLieuCauHoiController : ControllerBase
    {
        private readonly AppDbContext _context;
        public HocLieuCauHoiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] HocLieuCauHoi req)
        {
            _context.HocLieuCauHois.Add(req);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Thêm câu hỏi vào học liệu thành công" });
        }

        [HttpDelete("{maHocLieu:int}/{maCauHoi:int}")]
        public async Task<IActionResult> Delete(int maHocLieu, int maCauHoi)
        {
            var item = await _context.HocLieuCauHois
                .FirstOrDefaultAsync(x => x.MaHocLieu == maHocLieu && x.MaCauHoi == maCauHoi);

            if (item == null) return NotFound();

            _context.HocLieuCauHois.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa câu hỏi khỏi học liệu thành công" });
        }
    }
}
