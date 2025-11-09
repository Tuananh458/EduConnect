using EduConnect.Services.Interfaces;
using EduConnect.Shared.DTOs.HocSinh;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHocSinhRequest req)
        {
            var ok = await _service.CreateAsync(req);
            return ok ? Ok() : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok() : BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateHocSinhRequest req)
        {
            if (id != req.MaHocSinh) return BadRequest();
            var ok = await _service.UpdateAsync(req);
            if (!ok) return BadRequest("Cập nhật thất bại");
            return Ok(new { message = "Cập nhật thành công" });
        }

        // 📥 IMPORT DANH SÁCH HỌC SINH
        // ==========================================================
        [HttpGet("template")]
        public IActionResult DownloadTemplate()
        {
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("MauImport");
            ws.Cells[1, 1].Value = "Mã định danh";
            ws.Cells[1, 2].Value = "Họ và tên";
            ws.Cells[1, 3].Value = "Ngày sinh";
            ws.Cells[1, 4].Value = "Tên đăng nhập";
            ws.Cells[1, 5].Value = "Mật khẩu";
            ws.Cells["A1:E1"].Style.Font.Bold = true;
            ws.Cells["A:E"].AutoFitColumns();

            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MauImportHocSinh.xlsx");
        }

        [HttpPost("import/preview")]
        public async Task<IActionResult> Preview(IFormFile file,
                                                 [FromQuery] int maLopHoc,
                                                 [FromQuery] string? prefix = null,
                                                 [FromQuery] string? defaultPassword = null)
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
        // ==========================================================
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





    }
}
