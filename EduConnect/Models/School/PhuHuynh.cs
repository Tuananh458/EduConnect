using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduConnect.Models;
using EduConnect.Models.Auth;
using EduConnect.Models.School;

[Table("PHUHUYNH")]
public class PhuHuynh
{
    [Key]
    [Column("maPhuHuynh")]
    public int MaPhuHuynh { get; set; }

    [Required]
    [Column("maNguoiDung")]
    public Guid MaNguoiDung { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung NguoiDung { get; set; } = default!;

    [Column("ngheNghiep"), MaxLength(100)]
    public string? NgheNghiep { get; set; }

    [Column("diaChi"), MaxLength(255)]
    public string? DiaChi { get; set; }

    [Column("mucDoLienKet"), MaxLength(20)]
    public string? MucDoLienKet { get; set; }

    [Column("ngayTao")]
    public DateTime? NgayTao { get; set; } = DateTime.UtcNow;

    public ICollection<LienKetPhuHuynhHocSinh> LienKetPhuHuynhHocSinhs { get; set; } = new List<LienKetPhuHuynhHocSinh>();
}
