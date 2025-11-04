using System;

namespace EduConnect.Shared.DTOs.HocLieu
{
    public class HocLieuListDto
    {
        public int Id { get; set; }
        public string TenHocLieu { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
        public string TheLoai { get; set; } = string.Empty;
        public string? TenKhoaHoc { get; set; }
    }
}
