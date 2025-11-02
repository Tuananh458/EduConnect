using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.School
{
    [Table("LIENKET_PHUHUYNH_HOCSINH")]
    public class LienKetPhuHuynhHocSinh
    {
        [Key]
        [Column("maLienKet")]
        public int MaLienKet { get; set; }

        [Column("maPhuHuynh")]
        public int MaPhuHuynh { get; set; }

        [ForeignKey("MaPhuHuynh")]
        public PhuHuynh PhuHuynh { get; set; }

        [Column("maHocSinh")]
        public int MaHocSinh { get; set; }

        [ForeignKey("MaHocSinh")]
        public HocSinh HocSinh { get; set; }

        [Column("moiQuanHe"), MaxLength(20)]
        public string MoiQuanHe { get; set; } // Cha / Mẹ / Giám hộ

        [Column("trangThai")]
        public bool TrangThai { get; set; } = true;
    }
}
