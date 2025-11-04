using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.DanhMuc
{
    [Table("DM_MON")]
    public class DmMon
    {
        [Key]
        [Column("maMon")]
        public int MaMon { get; set; }

        [Column("tenMon"), MaxLength(150)]
        public string TenMon { get; set; } = string.Empty;

        [Column("maLop")]
        public int? MaLop { get; set; }
    }
}
