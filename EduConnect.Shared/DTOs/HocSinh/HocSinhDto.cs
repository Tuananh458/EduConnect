namespace EduConnect.Shared.DTOs.HocSinh
{
    public class HocSinhDto
    {
        public int MaHocSinh { get; set; }
        public Guid UserId { get; set; }
        public int MaLopHoc { get; set; }
        public string HoVaTen { get; set; } = "";
        public string TenDangNhap { get; set; } = "";
        public string MatKhau { get; set; } = "******";
        public string? MaDinhDanh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool LaLopTruong { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public class CreateHocSinhRequest
    {
        public int MaLopHoc { get; set; }
        public string HoVaTen { get; set; } = "";
        public string TenDangNhap { get; set; } = "";
        public string MatKhau { get; set; } = "";
        public string? MaDinhDanh { get; set; }
        public DateTime? NgaySinh { get; set; }
    }
    public class UpdateHocSinhRequest
    {
        public int MaHocSinh { get; set; }         // ID học sinh
        public int MaLopHoc { get; set; }          // Liên kết lớp
        public string HoVaTen { get; set; } = "";  // Họ và tên
        public string TenDangNhap { get; set; } = "";
        public string MatKhau { get; set; } = "";  // Mật khẩu (có thể bỏ trống nếu không đổi)
        public string? MaDinhDanh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool LaLopTruong { get; set; }
    }

}
