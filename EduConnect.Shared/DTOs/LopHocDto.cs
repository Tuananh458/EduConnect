using System;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Shared.DTOs
{
    public class LopHocDto
    {
        public int MaLopHoc { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống")]
        public string TenLopHoc { get; set; } = string.Empty;
        public string TenLopDayDu => $"{MaKhoiHoc}{TenLopHoc}";


        [Required(ErrorMessage = "Phải chọn khối học")]
        public int MaKhoiHoc { get; set; }

        public int? SiSo { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? TenKhoiHoc { get; set; }
    }

    // 🟢 Request tạo lớp (có thể nhập nhiều tên cùng lúc, VD: "A1;A2;A3")
    public class CreateLopHocRequest
    {
        [Required(ErrorMessage = "Phải chọn khối học")]
        public int MaKhoiHoc { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống")]
        public string TenLopHoc { get; set; } = string.Empty;
    }

    // 🟡 Request cập nhật lớp
    public class UpdateLopHocRequest
    {
        [Required]
        public int MaLopHoc { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống")]
        public string TenLopHoc { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phải chọn khối học")]
        public int MaKhoiHoc { get; set; }

        public int? SiSo { get; set; }
        public string? TrangThai { get; set; }
    }
}
