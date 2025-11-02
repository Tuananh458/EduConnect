using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.Quiz
{
    [Table("CHUONG")]
    public class Chuong
    {
        [Key]
        [Column("maChuong")]
        public int MaChuong { get; set; }

        [Required, MaxLength(255)]
        [Column("tenChuong")]
        public string TenChuong { get; set; } = string.Empty;

        public ICollection<CauHoi> CauHois { get; set; } = new List<CauHoi>();
    }
}
