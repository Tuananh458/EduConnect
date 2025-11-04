using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.DanhMuc
{
    [Table("DM_LOP")]
    public class DmLop
    {
        [Key]
        [Column("maLop")]
        public int MaLop { get; set; }

        [Column("tenLop"), MaxLength(100)]
        public string TenLop { get; set; } = string.Empty;
    }
}
