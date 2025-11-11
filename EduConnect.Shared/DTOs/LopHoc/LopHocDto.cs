using System;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Shared.DTOs.LopHoc
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
        public Guid? NguoiTaoId { get; set; }
        public string? TenNguoiTao { get; set; }
        public int? MaGiaoVienChuNhiem { get; set; }
        public string? TenGiaoVienChuNhiem { get; set; }
    }

    // 🟢 Request tạo lớp (có thể nhập nhiều tên cùng lúc, VD: "A1;A2;A3")
    public class CreateLopHocRequest
    {
        [Required(ErrorMessage = "Phải chọn khối học")]
        public int MaKhoiHoc { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống")]
        public string TenLopHoc { get; set; } = string.Empty;

        public int? SiSo { get; set; }
        public string? TrangThai { get; set; } = "Hoạt động";

        public Guid NguoiTaoId { get; set; }
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
        public Guid? NguoiTaoId { get; set; }
    }
}
