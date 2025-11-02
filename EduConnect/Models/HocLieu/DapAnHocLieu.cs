using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.HocLieu
{
    public class DapAnHocLieu
    {
        [Key]
        public int MaDapAn { get; set; }

        [ForeignKey(nameof(CauHoiHocLieu))]
        public int MaCauHoi { get; set; }

        [Required]
        public string NoiDung { get; set; } = string.Empty;

        public bool LaDapAnDung { get; set; }
        public int ThuTu { get; set; }

        public CauHoiHocLieu CauHoiHocLieu { get; set; } = null!;
    }
}
