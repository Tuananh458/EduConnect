namespace EduConnect.Shared.DTOs.HocLieu
{
    public class CauHoiNganHangDto
    {
        public int Id { get; set; }
        public string TieuDe { get; set; } = "";
        public string NoiDung { get; set; } = "";
        public string LoaiCauHoi { get; set; } = "TN_1DAPANDUNG";
        public string DoKho { get; set; } = "Nhận biết";
        public double Diem { get; set; }
        public string? DapAnA { get; set; }
        public string? DapAnB { get; set; }
        public string? DapAnC { get; set; }
        public string? DapAnD { get; set; }
        public string? DapAnDung { get; set; }
        public int? ThuTu { get; set; }
    }
}
