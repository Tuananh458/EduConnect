namespace EduConnect.Shared.DTOs.LopHoc
{
    public class LopHocChiTietDto
    {
        public int MaLopHoc { get; set; }
        public string TenLopHoc { get; set; } = string.Empty;
        public string TenKhoiHoc { get; set; } = string.Empty;
        public int? SiSo { get; set; }

        // 🧑‍🏫 Giáo viên chủ nhiệm
        public string? TenGiaoVienChuNhiem { get; set; }
        public string? EmailGiaoVien { get; set; }
        public string? AvatarGiaoVien { get; set; }

        // 👥 Danh sách học sinh trong lớp
        public List<HocSinhTrongLopDto> DanhSachHocSinh { get; set; } = new();
    }

    public class HocSinhTrongLopDto
    {
        public int MaHocSinh { get; set; }
        public string HoVaTen { get; set; } = string.Empty;
        public bool LaLopTruong { get; set; }
    }
}
