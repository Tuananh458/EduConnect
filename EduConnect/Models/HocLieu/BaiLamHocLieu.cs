using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.HocLieu
{
    [Table("BAILAMHOCLIEU")]
    public class BaiLamHocLieu
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("hocLieuId")]
        public int HocLieuId { get; set; }

        [Column("hocSinhId")]
        public int HocSinhId { get; set; }

        [Column("ngayBatDau")]
        public DateTime? NgayBatDau { get; set; }

        [Column("ngayNop")]
        public DateTime? NgayNop { get; set; }

        [Column("diem")]
        public double? Diem { get; set; }

        [Column("trangThai")]
        [MaxLength(50)]
        public string TrangThai { get; set; } = "Đang làm";

        // Navs
        [ForeignKey(nameof(HocLieuId))]
        public HocLieu HocLieu { get; set; } = null!;

        [ForeignKey(nameof(HocSinhId))]
        public HocSinh HocSinh { get; set; } = null!;

        // ✅ dùng chính BaiLamChiTiet làm collection
        public ICollection<BaiLamChiTiet> ChiTiets { get; set; } = new List<BaiLamChiTiet>();
    }
}
