namespace EduConnect.Shared.DTOs.HocLieu
{
    public class UpdateHocLieuRequest
    {
        public string TenHocLieu { get; set; } = string.Empty;
        public int? KhoaHocId { get; set; }
        public bool LaHocLieuTuDo { get; set; }
        public bool LaHocLieuAn { get; set; }
        public string? TrangThai { get; set; }
    }
}
