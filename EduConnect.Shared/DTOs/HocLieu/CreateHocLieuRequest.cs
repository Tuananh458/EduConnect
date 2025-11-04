namespace EduConnect.Shared.DTOs.HocLieu
{
    public class CreateHocLieuRequest
    {
        public string TenHocLieu { get; set; } = string.Empty;
        public string MaLoaiHocLieu { get; set; } = string.Empty;
        public string? TenLoaiHocLieu { get; set; }
        public string NguonTao { get; set; } = "Tự tạo";
        public bool LaHocLieuTuDo { get; set; }
        public bool LaHocLieuAn { get; set; }
    }
}
