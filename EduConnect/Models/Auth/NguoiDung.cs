using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models
{
    [Table("NGUOIDUNG")]
    public class NguoiDung
    {
        [Key]
        [Column("maNguoiDung")]
        public Guid MaNguoiDung { get; set; } = Guid.NewGuid();

        [Required]
        [Column("tenDangNhap"), MaxLength(50)]
        public string TenDangNhap { get; set; } = default!;

        [Required]
        [Column("hoTen"), MaxLength(100)]
        public string HoTen { get; set; } = default!;

        [Column("email"), MaxLength(100)]
        public string? Email { get; set; }

        [Column("soDienThoai"), MaxLength(15)]
        public string? SoDienThoai { get; set; }

        [Column("anhDaiDien"), MaxLength(255)]
        public string? AnhDaiDien { get; set; } = "/template/img/avt.svg";

        [Column("matKhauHash")]
        public string MatKhauHash { get; set; } = string.Empty;

        [Column("vaiTro"), MaxLength(20)]
        public string VaiTro { get; set; } = "HocSinh";

        [Column("nguonXacThuc"), MaxLength(20)]
        public string NguonXacThuc { get; set; } = "Local";

        [Column("phaiDoiMatKhau")]
        public bool PhaiDoiMatKhau { get; set; } = true;

        [Column("trangThai")]
        public byte TrangThai { get; set; } = 1;

        [Column("ngayTao")]
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        [Column("lanDangNhapCuoi")]
        public DateTime? LanDangNhapCuoi { get; set; }

        // Quan hệ 1-n
        public List<TokenLamMoi> TokenLamMois { get; set; } = new();

        // Quan hệ 1-1
        public HocSinh? HocSinh { get; set; }
        public GiaoVien? GiaoVien { get; set; }
        public PhuHuynh? PhuHuynh { get; set; }
    }
}
