namespace EduConnect.Shared.DTOs.HocSinh
{
    // ==============================
    // 📘 Dữ liệu Học Sinh
    // ==============================
    public class HocSinhDto
    {
        public int MaHocSinh { get; set; }
        public Guid MaNguoiDung { get; set; }  // 🔁 Đổi từ UserId → MaNguoiDung

        public int? MaLopHoc { get; set; }

        public string HoVaTen { get; set; } = "";
        public string TenDangNhap { get; set; } = "";
        public string MatKhau { get; set; } = "******";
        public string? MaDinhDanh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool LaLopTruong { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    // ==============================
    // ➕ Yêu cầu tạo học sinh mới
    // ==============================
    public class CreateHocSinhRequest
    {
        public int? MaLopHoc { get; set; }

        public string HoVaTen { get; set; } = "";
        public string TenDangNhap { get; set; } = "";
        public string MatKhau { get; set; } = "";
        public string? MaDinhDanh { get; set; }
        public DateTime? NgaySinh { get; set; }
    }

    // ==============================
    // ✏️ Yêu cầu cập nhật học sinh
    // ==============================
    public class UpdateHocSinhRequest
    {
        public int MaHocSinh { get; set; }
        public int? MaLopHoc { get; set; }

        public string HoVaTen { get; set; } = "";
        public string TenDangNhap { get; set; } = "";
        public string MatKhau { get; set; } = "";
        public string? MaDinhDanh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool LaLopTruong { get; set; }
    }
}
