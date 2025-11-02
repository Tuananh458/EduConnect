using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models.School
{
    [Table("GIAOVIEN")]
    public class GiaoVien
    {
        [Key]
        [Column("maGiaoVien")]
        public int MaGiaoVien { get; set; }

        [Column("maNguoiDung")]
        public int MaNguoiDung { get; set; }

        [Column("chuyenMon"), MaxLength(100)]
        public string ChuyenMon { get; set; }

        [Column("trinhDo"), MaxLength(60)]
        public string TrinhDo { get; set; }

        [Column("ngayBatDau")]
        public DateTime? NgayBatDau { get; set; }

        [Column("diaChi"), MaxLength(255)]
        public string DiaChi { get; set; }

        [Column("trangThaiCongTac")]
        public bool TrangThaiCongTac { get; set; } = true;

        // 🔗 Navigation
        public ICollection<LopHoc> LopHocsChuNhiem { get; set; }
    }
}
