using System;

namespace EduConnect.Shared.DTOs.HocLieu
{
    public class HocLieuDto
    {
        public int MaHocLieu { get; set; }
        public string TenHocLieu { get; set; } = string.Empty;
        public string TheLoai { get; set; } = string.Empty;
        public bool DaDuyet { get; set; }
        public bool HienThi { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
