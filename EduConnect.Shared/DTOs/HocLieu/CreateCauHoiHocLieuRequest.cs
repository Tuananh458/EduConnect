using System.Collections.Generic;

namespace EduConnect.Shared.DTOs.HocLieu
{
    public class CreateCauHoiHocLieuRequest
    {
        // tạo trực tiếp trong học liệu
        public int? MaHocLieu { get; set; }

        public string NoiDung { get; set; } = string.Empty;
        public LoaiCauHoiHocLieu Loai { get; set; } = LoaiCauHoiHocLieu.TracNghiem;
        public string DoKho { get; set; } = "Nhận biết";
        public string? GiaiThich { get; set; }
    }
}
