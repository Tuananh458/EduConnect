using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.School
{
    [Table("PHUHUYNH")]
    public class PhuHuynh
    {
        [Key]
        [Column("maPhuHuynh")]
        public int MaPhuHuynh { get; set; }

        [Column("maNguoiDung")]
        public int MaNguoiDung { get; set; }

        [Column("ngheNghiep"), MaxLength(100)]
        public string NgheNghiep { get; set; }

        [Column("diaChi"), MaxLength(255)]
        public string DiaChi { get; set; }

        [Column("mucDoLienKet"), MaxLength(20)]
        public string MucDoLienKet { get; set; }

        // 🔗 Navigation
        public ICollection<LienKetPhuHuynhHocSinh> LienKetPhuHuynhHocSinhs { get; set; }
    }
}
