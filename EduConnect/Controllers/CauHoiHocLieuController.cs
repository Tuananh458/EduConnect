using System.Collections.Generic;
using System.Threading.Tasks;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocLieu;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CauHoiHocLieuController : ControllerBase
    {
        private readonly ICauHoiHocLieuService _service;

        public CauHoiHocLieuController(ICauHoiHocLieuService service)
        {
            _service = service;
        }

        // GET api/CauHoiHocLieu?hocLieuId=1
        [HttpGet]
        public async Task<ActionResult<List<CauHoiHocLieuDto>>> GetByHocLieu([FromQuery] int hocLieuId)
        {
            var data = await _service.GetByHocLieuAsync(hocLieuId);
            return Ok(data);
        }

        // GET api/CauHoiHocLieu/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CauHoiHocLieuDto>> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST api/CauHoiHocLieu
        [HttpPost]
        public async Task<ActionResult<CauHoiHocLieuDto>> Create([FromBody] CreateCauHoiHocLieuRequest request)
        {
            var created = await _service.CreateAsync(request);
            return Ok(created);
        }

        // PUT api/CauHoiHocLieu/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCauHoiHocLieuRequest request)
        {
            var ok = await _service.UpdateAsync(id, request);
            if (!ok) return NotFound();
            return NoContent();
        }

        // DELETE api/CauHoiHocLieu/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

