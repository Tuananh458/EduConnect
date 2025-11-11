using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocSinh;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HocSinhController : ControllerBase
    {
        private readonly IHocSinhService _service;

        public HocSinhController(IHocSinhService service)
        {
            _service = service;
        }

        [HttpGet("lop/{maLopHoc}")]
        public async Task<IActionResult> GetByLop(int maLopHoc)
        {
            var result = await _service.GetByLopAsync(maLopHoc);
            return Ok(result);
        }

        [Authorize(Roles = "GiaoVien,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHocSinhRequest req)
        {
            var ok = await _service.CreateAsync(req);
            return ok ? Ok() : BadRequest();
        }

        [Authorize(Roles = "GiaoVien,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok() : BadRequest();
        }

        [Authorize(Roles = "GiaoVien,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateHocSinhRequest req)
        {
            if (id != req.MaHocSinh) return BadRequest();
            var ok = await _service.UpdateAsync(req);
            return ok ? Ok(new { message = "Cập nhật thành công" }) : BadRequest("Cập nhật thất bại");
        }

        // 📥 IMPORT DANH SÁCH HỌC SINH
        [HttpGet("template")]
        public IActionResult DownloadTemplate()
        {
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("MauImport");

            // 🔹 Tiêu đề cột
            ws.Cells[1, 1].Value = "Mã định danh";
            ws.Cells[1, 2].Value = "Họ và tên";
            ws.Cells[1, 3].Value = "Ngày sinh";
            ws.Cells[1, 4].Value = "Tên đăng nhập";
            ws.Cells[1, 5].Value = "Mật khẩu";
            ws.Cells["A1:E1"].Style.Font.Bold = true;

            // 🔹 Ví dụ minh họa (dòng 2)
            ws.Cells[2, 1].Value = "HS001";
            ws.Cells[2, 2].Value = "Nguyễn Văn A";
            ws.Cells[2, 3].Value = "15/09/2007";
            ws.Cells[2, 4].Value = "nguyenvana";
            ws.Cells[2, 5].Value = "123456";

            ws.Cells["A:E"].AutoFitColumns();

            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MauImportHocSinh.xlsx");
        }

        [Authorize(Roles = "GiaoVien,Admin")]
        [HttpPost("import/preview")]
        public async Task<IActionResult> Preview(IFormFile file, [FromQuery] int maLopHoc,
                                                 [FromQuery] string? prefix = null, [FromQuery] string? defaultPassword = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Vui lòng chọn file hợp lệ.");

                var result = await _service.GeneratePreviewAsync(file, maLopHoc, prefix, defaultPassword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi xử lý file: {ex.Message}");
            }
        }
        [Authorize(Roles = "GiaoVien,Admin")]
        [HttpPost("import/confirm")]
        public async Task<IActionResult> Confirm([FromBody] ImportConfirmRequest req)
        {
            try
            {
                if (req.Items == null || req.Items.Count == 0)
                    return BadRequest("Danh sách trống, không có dữ liệu import.");

                var result = await _service.ConfirmImportAsync(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi import: {ex.Message}");
            }
        }

        // 🗑️ XOÁ HÀNG LOẠT HỌC SINH
        [Authorize(Roles = "GiaoVien,Admin")]
        [HttpDelete("batch")]
        public async Task<IActionResult> DeleteMany([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("Không có học sinh nào được chọn.");

            try
            {
                var count = await _service.DeleteManyAsync(ids);
                return Ok(new { message = $"🗑️ Đã xóa {count} học sinh.", deleted = count });
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi xóa hàng loạt: {ex.Message}");
            }
        }

        // 🧩 LẤY THÔNG TIN HỌC SINH HIỆN TẠI
        [Authorize(Roles = "HocSinh")]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentHocSinh()
        {
            try
            {
                var userIdClaim = User.FindFirst("sub")
                                 ?? User.FindFirst("nameid")
                                 ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    return Unauthorized("Không tìm thấy thông tin người dùng trong token.");

                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                    return BadRequest("Định dạng ID người dùng không hợp lệ.");

                var hocSinh = await _service.GetByUserIdAsync(userId);
                if (hocSinh == null)
                    return NotFound("Không tìm thấy học sinh tương ứng với tài khoản này.");

                return Ok(new
                {
                    hocSinh.MaHocSinh,
                    hocSinh.HoVaTen,
                    hocSinh.MaLopHoc
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy thông tin học sinh: {ex.Message}");
            }
        }

        // 🧩 LẤY THÔNG TIN LỚP HỌC CỦA HỌC SINH HIỆN TẠI
        [Authorize]
        [HttpGet("lop-cua-toi")]
        public async Task<IActionResult> GetLopCuaToi()
        {
            try
            {
                var userIdClaim = User.FindFirst("nameid") ??
                                  User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) ??
                                  User.FindFirst("sub");

                if (userIdClaim == null)
                    return Unauthorized("Không tìm thấy thông tin người dùng trong token.");

                var userId = Guid.Parse(userIdClaim.Value);
                var result = await _service.GetLopHocCuaToiAsync(userId);

                if (result == null)
                    return NotFound("Bạn hiện chưa được xếp vào lớp nào.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy thông tin lớp học: {ex.Message}");
            }
        }


    }
}
