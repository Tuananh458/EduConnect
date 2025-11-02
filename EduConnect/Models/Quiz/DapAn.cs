using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.Quiz
{
    [Table("DAPAN")]
    public class DapAn
    {
        [Key]
        [Column("maDapAn")]
        public int MaDapAn { get; set; }

        [Required]
        [Column("noiDung")]
        [MaxLength(2000)]
        public string NoiDung { get; set; } = string.Empty;

        [Column("isDung")]
        public bool IsDung { get; set; }

        [Column("thuTu")]
        public int ThuTu { get; set; }

        [Column("maCauHoi")]
        public int MaCauHoi { get; set; }

        [ForeignKey(nameof(MaCauHoi))]
        public CauHoi CauHoi { get; set; }

        [Column("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }
}
