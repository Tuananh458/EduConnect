namespace EduConnect.Shared.DTOs.HocLieu
{
    public class HocLieuFilterDto
    {
        public string MaLoaiHocLieu { get; set; } = "all";
        public string NguonTao { get; set; } = "all";
        public string? Keyword { get; set; }
        public bool LaHocLieuTuDo { get; set; }
        public bool LaHocLieuAn { get; set; }
    }
}
