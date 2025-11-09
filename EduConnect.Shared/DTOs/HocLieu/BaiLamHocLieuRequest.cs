namespace EduConnect.Shared.DTOs.HocLieu
{
    public class BaiLamHocLieuRequest
    {
        public int HocLieuId { get; set; }
        public string TenHocSinh { get; set; } = string.Empty;
        public List<BaiLamCauHoiItem> CauHoiDaLam { get; set; } = new();
    }

    public class BaiLamCauHoiItem
    {
        public int CauHoiId { get; set; }
        public string? DapAnChon { get; set; }
    }
}
