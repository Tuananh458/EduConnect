using System;
using System.Collections.Generic; 

namespace EduConnect.Shared.DTOs.HocLieu
{
    public class HocLieuDto
    {
        public int Id { get; set; }
        public string TenHocLieu { get; set; } = string.Empty;
        public string MaLoaiHocLieu { get; set; } = string.Empty;
        public string? TenLoaiHocLieu { get; set; }
        public string NguonTao { get; set; } = "Tự tạo";
        public bool LaHocLieuTuDo { get; set; }
        public bool LaHocLieuAn { get; set; }
        public DateTime NgayTao { get; set; }
        public string? TenKhoaHoc { get; set; }
        public Guid NguoiTaoId { get; set; }


   
        public List<CauHoiHocLieuDto> CauHoiTuTao { get; set; } = new();
        public List<CauHoiNganHangDto> CauHoiDuocChon { get; set; } = new();

    }
}
