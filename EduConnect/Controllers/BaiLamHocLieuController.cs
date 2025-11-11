using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaiLamHocLieuController : ControllerBase
    {
        private readonly IBaiLamHocLieuService _service;

        public BaiLamHocLieuController(IBaiLamHocLieuService service)
        {
            _service = service;
        }

        [HttpGet("dethi/{hocLieuId:int}")]
        public async Task<IActionResult> DeThi(int hocLieuId)
        {
            var rs = await _service.GetDeThiAsync(hocLieuId);
            return rs == null ? NotFound(new { Message = "Không tìm thấy học liệu" }) : Ok(rs);
        }

        [HttpPost("tao")]
        public async Task<IActionResult> Tao([FromBody] TaoBaiLamRequest dto)
        {
            if (dto == null || dto.HocLieuId <= 0 || dto.HocSinhId <= 0)
                return BadRequest(new { Message = "Dữ liệu không hợp lệ" });

            var id = await _service.TaoBaiLamAsync(dto);
            return id > 0 ? Ok(new { BaiLamId = id }) : StatusCode(500, new { Message = "Không thể tạo bài làm" });
        }

        [HttpPost("nop")]
        public async Task<IActionResult> Nop([FromBody] NopBaiRequest dto)
        {
            if (dto == null || dto.HocLieuId <= 0 || dto.HocSinhId <= 0 || dto.CauTraLoi.Count == 0)
                return BadRequest(new { Message = "Dữ liệu nộp bài không hợp lệ" });

            var ok = await _service.NopBaiAsync(dto);
            return ok ? Ok(new { Message = "Nộp bài thành công!" }) : StatusCode(500, new { Message = "Nộp bài thất bại!" });
        }

        [HttpGet("{baiLamId:int}")]
        public async Task<IActionResult> ChiTiet(int baiLamId)
        {
            var rs = await _service.GetChiTietAsync(baiLamId);
            return rs == null ? NotFound(new { Message = "Không tìm thấy bài làm" }) : Ok(rs);
        }

        [HttpGet("ketqua/hoclieu/{hocLieuId:int}")]
        public async Task<IActionResult> KetQuaHocLieu(int hocLieuId)
        {
            var rs = await _service.GetKetQuaTheoHocLieuAsync(hocLieuId);
            return Ok(rs ?? Enumerable.Empty<BaiLamHocLieuDto>());
        }
    }
}
