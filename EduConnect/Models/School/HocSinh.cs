using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.School
{
    [Table("HOCSINH")]
    public class HocSinh
    {
        [Key]
        [Column("maHocSinh")]
        public int MaHocSinh { get; set; }

        [Column("maNguoiDung")]
        public int MaNguoiDung { get; set; }

        [Column("maLopHoc")]
        public int MaLopHoc { get; set; }

        [ForeignKey("MaLopHoc")]
        public LopHoc LopHoc { get; set; }

        [Column("ngaySinh")]
        public DateTime? NgaySinh { get; set; }

        [Column("gioiTinh"), MaxLength(10)]
        public string GioiTinh { get; set; }

        [Column("diaChi"), MaxLength(255)]
        public string DiaChi { get; set; }

        [Column("ngayNhapHoc")]
        public DateTime? NgayNhapHoc { get; set; }

        [Column("trangThai"), MaxLength(20)]
        public string TrangThai { get; set; } = "Đang học";

        // 🔗 Navigation
        public ICollection<LienKetPhuHuynhHocSinh> LienKetPhuHuynhHocSinhs { get; set; }
    }
}
