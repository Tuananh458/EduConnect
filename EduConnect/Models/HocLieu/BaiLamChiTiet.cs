using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.HocLieu
{
    [Table("BAILAMCHITIET")]
    public class BaiLamChiTiet
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("maBaiLam")]
        public int MaBaiLam { get; set; }

        [Required]
        [Column("maCauHoi")]
        public int MaCauHoi { get; set; } // FK -> CauHoiHocLieu.Id

        [Column("traLoi")]
        public string? TraLoi { get; set; } // "A" | "A;C" | text ...

        [Column("dungSai")]
        public bool? DungSai { get; set; }

        [ForeignKey(nameof(MaBaiLam))]
        public BaiLamHocLieu BaiLamHocLieu { get; set; } = null!;

        [ForeignKey(nameof(MaCauHoi))]
        public CauHoiHocLieu? CauHoiHocLieu { get; set; }
    }
}
