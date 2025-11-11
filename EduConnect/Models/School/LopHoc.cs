using EduConnect.Models.School;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models
{
    [Table("LOPHOC")]
    [Index(nameof(MaKhoiHoc), nameof(TenLopHoc), IsUnique = true)]
    public class LopHoc
    {
        [Key]
        [Column("maLopHoc")]
        public int MaLopHoc { get; set; }

        [Required]
        [Column("tenLopHoc")]
        public string TenLopHoc { get; set; } = string.Empty;

        [Required]
        [Column("maKhoiHoc")]
        public int MaKhoiHoc { get; set; }

        [Column("siSo")]
        public int? SiSo { get; set; }

        [Column("trangThai")]
        public string? TrangThai { get; set; }

        [Column("ngayTao")]
        public DateTime? NgayTao { get; set; }

        [Column("nguoiTaoId")]
        public Guid? NguoiTaoId { get; set; }  

        // 🔗 Khóa ngoại tới người dùng
        [ForeignKey(nameof(NguoiTaoId))]
        public NguoiDung? NguoiTao { get; set; }

        // 🔗 Khóa ngoại tới KHOIHOC
        [ForeignKey("MaKhoiHoc")]
        public KhoiHoc? KhoiHoc { get; set; }

        [Column("maGiaoVienChuNhiem")]
        public int? MaGiaoVienChuNhiem { get; set; }

        [ForeignKey(nameof(MaGiaoVienChuNhiem))]
        public GiaoVien? GiaoVienChuNhiem { get; set; }
    }
}
