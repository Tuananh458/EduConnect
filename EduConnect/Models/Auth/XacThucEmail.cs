using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduConnect.Models;
using EduConnect.Models.Auth;

[Table("XACTHUCEMAIL")]
public class XacThucEmail
{
    [Key]
    [Column("maXacThuc")]
    public Guid MaXacThuc { get; set; } = Guid.NewGuid();

    [Required]
    [Column("maNguoiDung")]
    public Guid MaNguoiDung { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung NguoiDung { get; set; } = default!;

    [Required]
    [Column("maCode"), MaxLength(50)]
    public string MaCode { get; set; } = string.Empty;

    [Column("ngayHetHan")]
    public DateTime NgayHetHan { get; set; }

    [Column("daSuDung")]
    public bool DaSuDung { get; set; } = false;

    [Column("ngayTao")]
    public DateTime NgayTao { get; set; } = DateTime.UtcNow;
}
