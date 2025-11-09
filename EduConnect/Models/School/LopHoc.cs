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

        // Optional: liên kết tới bảng KHOIHOC (nếu có)
        [ForeignKey("MaKhoiHoc")]
        public KhoiHoc? KhoiHoc { get; set; }
    }
}
