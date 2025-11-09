using EduConnect.Models.Auth;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models
{
    [Table("HOCSINH")]
    public class HocSinh
    {
        [Key]
        [Column("maHocSinh")]
        public int MaHocSinh { get; set; }

        [Required]
        [Column("userId")]
        public Guid UserId { get; set; }

        [Required]
        [Column("maLopHoc")]
        public int MaLopHoc { get; set; }

        [Column("maDinhDanh")]
        public string? MaDinhDanh { get; set; }

        [Column("ngaySinh")]
        public DateTime? NgaySinh { get; set; }

        [Column("laLopTruong")]
        public bool LaLopTruong { get; set; } = false;

        [Column("ngayTao")]
        public DateTime? NgayTao { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [ForeignKey(nameof(MaLopHoc))]
        public LopHoc? LopHoc { get; set; }
    }
}
