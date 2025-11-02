using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.Quiz
{
    [Table("CHUDE")]
    public class ChuDe
    {
        [Key]
        [Column("maChuDe")]
        public int MaChuDe { get; set; }

        [Required, MaxLength(255)]
        [Column("tenChuDe")]
        public string TenChuDe { get; set; } = string.Empty;

        public ICollection<CauHoi> CauHois { get; set; } = new List<CauHoi>();
    }
}
