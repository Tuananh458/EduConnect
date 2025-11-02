using System.Text.Json.Serialization;

namespace EduConnect.Models.HocLieu
{
    // bảng trung gian: 1 học liệu - nhiều câu hỏi
    public class HocLieuCauHoi
    {
        public int MaHocLieu { get; set; }
        public int MaCauHoi { get; set; }
        public double Diem { get; set; } = 1;

        [JsonIgnore]
        public HocLieu? HocLieu { get; set; }

        [JsonIgnore]
        public CauHoiHocLieu? CauHoiHocLieu { get; set; }
    }
}
