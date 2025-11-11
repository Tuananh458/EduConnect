using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduConnect.Models;
using EduConnect.Models.Auth;
using EduConnect.Models.School;

[Table("HOCSINH")]
public class HocSinh
{
    [Key]
    [Column("maHocSinh")]
    public int MaHocSinh { get; set; }

    [Required]
    [Column("maNguoiDung")]
    public Guid MaNguoiDung { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung NguoiDung { get; set; } = default!;

    [Column("maLopHoc")]
    public int? MaLopHoc { get; set; }

    [ForeignKey(nameof(MaLopHoc))]
    public LopHoc? LopHoc { get; set; }

    [Column("maDinhDanh"), MaxLength(50)]
    public string? MaDinhDanh { get; set; }

    [Column("ngaySinh")]
    public DateTime? NgaySinh { get; set; }

    [Column("laLopTruong")]
    public bool LaLopTruong { get; set; } = false;

    [Column("ngayTao")]
    public DateTime NgayTao { get; set; } = DateTime.Now;
}
