using System;
using System.Collections.Generic;

namespace EduConnect.Shared.DTOs.HocLieu
{
    public class CauHoiHocLieuDto
    {
        public int MaCauHoi { get; set; }
        public string NoiDung { get; set; } = string.Empty;
        public LoaiCauHoiHocLieu Loai { get; set; }
        public string DoKho { get; set; } = "Nhận biết";
        public string? GiaiThich { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
