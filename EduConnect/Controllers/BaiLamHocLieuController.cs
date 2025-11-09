using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaiLamHocLieuController : ControllerBase
    {
        private readonly IBaiLamHocLieuService _svc;

        public BaiLamHocLieuController(IBaiLamHocLieuService svc)
        {
            _svc = svc;
        }

        [HttpPost("nopbai")]
        public async Task<IActionResult> NopBai([FromBody] BaiLamHocLieuRequest req)
        {
            var diem = await _svc.LuuBaiLamAsync(req);
            return Ok(new { message = "Lưu thành công", tongDiem = diem });
        }
    }
}
