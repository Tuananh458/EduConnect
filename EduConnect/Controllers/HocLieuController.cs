using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HocLieuController : ControllerBase
    {
        private readonly IHocLieuService _service;

        public HocLieuController(IHocLieuService service)
        {
            _service = service;
        }

        [Authorize(Roles = "GiaoVien")]
        [HttpGet]
        public async Task<ActionResult<List<HocLieuListDto>>> GetAll(
        [FromQuery] string? keyword,
        [FromQuery] string maLoaiHocLieu = "all",
        [FromQuery] string nguonTao = "all",
        [FromQuery] bool? laHocLieuTuDo = null,
        [FromQuery] bool? laHocLieuAn = null)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { Message = "Không tìm thấy thông tin người dùng." });

            var userId = Guid.Parse(userIdClaim);

            var filter = new HocLieuFilterDto
            {
                Keyword = keyword,
                MaLoaiHocLieu = maLoaiHocLieu,
                NguonTao = nguonTao,
                LaHocLieuTuDo = laHocLieuTuDo ?? false,
                LaHocLieuAn = laHocLieuAn ?? false,
                NguoiTaoId = userId
            };

            var data = await _service.GetAllAsync(filter);
            return Ok(data);
        }


        // GET api/HocLieu/5
        [Authorize(Roles = "GiaoVien")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HocLieuDto>> Get(int id)
        {
            var hl = await _service.GetByIdAsync(id);
            if (hl == null) return NotFound();
            return Ok(hl);
        }

        [Authorize(Roles = "GiaoVien")]
        [HttpPost]
        public async Task<ActionResult<HocLieuDto>> Create([FromBody] CreateHocLieuRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { Message = "Không tìm thấy thông tin người dùng." });

            var userId = Guid.Parse(userIdClaim);

            // ✅ Gán người tạo vào request (đảm bảo DTO có NguoiTaoId)
            request.NguoiTaoId = userId;

            var created = await _service.CreateAsync(request);
            return Ok(created);
        }


        // DELETE api/HocLieu/5
        [Authorize(Roles = "GiaoVien")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpGet("GetCauHoiTrongHocLieu/{hocLieuId:int}")]
        public async Task<IActionResult> GetCauHoiTrongHocLieu(int hocLieuId)
        {
            var cauHois = await _service.GetCauHoiTrongHocLieuAsync(hocLieuId);
            return Ok(cauHois);
        }
    }
}
