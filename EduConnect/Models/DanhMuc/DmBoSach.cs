using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.DanhMuc
{
    [Table("DM_BOSACH")]
    public class DmBoSach
    {
        [Key]
        [Column("maBoSach")]
        public int MaBoSach { get; set; }

        [Column("tenBoSach"), MaxLength(150)]
        public string TenBoSach { get; set; } = string.Empty;

        [Column("maLop")]
        public int? MaLop { get; set; }

        [Column("maMon")]
        public int? MaMon { get; set; }
    }
}
