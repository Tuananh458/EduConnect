using Microsoft.AspNetCore.Mvc;
using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs;
using System.Threading.Tasks;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LopHocController : ControllerBase
    {
        private readonly ILopHocService _service;

        public LopHocController(ILopHocService service)
        {
            _service = service;
        }

        // 🟢 Lấy danh sách lớp học
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // 🟢 Lấy chi tiết 1 lớp học theo ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy lớp học" });

            return Ok(result);
        }

        // 🟢 Thêm mới lớp học (cho phép tạo nhiều lớp cùng lúc, ví dụ: A1;A2)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLopHocRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TenLopHoc))
                return BadRequest(new { message = "Tên lớp học không được để trống" });

            await _service.CreateAsync(request);
            return Ok(new { message = "Thêm lớp học thành công" });
        }

        // 🟠 Cập nhật thông tin lớp học
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLopHocRequest request)
        {
            if (id != request.MaLopHoc)
                return BadRequest(new { message = "Mã lớp học không khớp" });

            await _service.UpdateAsync(request);
            return Ok(new { message = "Cập nhật lớp học thành công" });
        }

        // 🔴 Xóa lớp học
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Xóa lớp học thành công" });
        }
    }
}
