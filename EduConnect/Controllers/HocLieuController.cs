using System.Collections.Generic;
using System.Threading.Tasks;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
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

        // GET api/HocLieu
        [HttpGet]
        public async Task<ActionResult<List<HocLieuListDto>>> GetAll(
            [FromQuery] string? keyword,
            [FromQuery] string maLoaiHocLieu = "all",
            [FromQuery] string nguonTao = "all",
            [FromQuery] bool? laHocLieuTuDo = null,
            [FromQuery] bool? laHocLieuAn = null)
        {
            var filter = new HocLieuFilterDto
            {
                Keyword = keyword,
                MaLoaiHocLieu = maLoaiHocLieu,
                NguonTao = nguonTao,
                LaHocLieuTuDo = laHocLieuTuDo ?? false,
                LaHocLieuAn = laHocLieuAn ?? false
            };

            var data = await _service.GetAllAsync(filter);
            return Ok(data);
        }

        // GET api/HocLieu/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HocLieuDto>> Get(int id)
        {
            var hl = await _service.GetByIdAsync(id);
            if (hl == null) return NotFound();
            return Ok(hl);
        }

        // POST api/HocLieu
        [HttpPost]
        public async Task<ActionResult<HocLieuDto>> Create([FromBody] CreateHocLieuRequest request)
        {
            var created = await _service.CreateAsync(request);
            return Ok(created);
        }

        // DELETE api/HocLieu/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
