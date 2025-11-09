using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HocLieuCauHoiController : ControllerBase
    {
        private readonly IHocLieuCauHoiService _service;

        public HocLieuCauHoiController(IHocLieuCauHoiService service)
        {
            _service = service;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] HocLieuCauHoiRequest req)
        {
            await _service.SaveHocLieuCauHoiAsync(req);
            return Ok(new { success = true });
        }

        [HttpGet("{hocLieuId}")]
        public async Task<IActionResult> Get(int hocLieuId)
        {
            var ids = await _service.GetSelectedCauHoiIdsAsync(hocLieuId);
            return Ok(ids);
        }
    }
}
