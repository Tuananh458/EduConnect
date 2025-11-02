using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.Quiz
{
    [Table("CAUHOI")]
    public class CauHoi
    {
        [Key]
        [Column("maCauHoi")]
        public int MaCauHoi { get; set; }

        [Required]
        [Column("noiDung")]
        [MaxLength(4000)]
        public string NoiDung { get; set; } = string.Empty;

        [Column("doKho")]
        [MaxLength(50)]
        public string DoKho { get; set; } = "Nhận biết";

        [Column("maChuong")]
        public int? MaChuong { get; set; }

        [Column("maChuDe")]
        public int? MaChuDe { get; set; }

        [Column("createdBy")]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        [Column("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        // ✅ Thêm navigation property để fix lỗi .Include(x => x.Chuong / ChuDe)
        [ForeignKey("MaChuong")]
        public Chuong? Chuong { get; set; }

        [ForeignKey("MaChuDe")]
        public ChuDe? ChuDe { get; set; }

        public ICollection<DapAn> DapAns { get; set; } = new List<DapAn>();
    }
}
