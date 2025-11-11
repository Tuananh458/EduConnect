using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models
{
    [Table("DATLAIMATKHAU")]
    public class DatLaiMatKhau
    {
        [Key]
        [Column("maDatLaiMatKhau")]
        public Guid MaDatLaiMatKhau { get; set; } = Guid.NewGuid();

        [Required]
        [Column("maNguoiDung")]
        public Guid MaNguoiDung { get; set; }

        [ForeignKey(nameof(MaNguoiDung))]
        public NguoiDung NguoiDung { get; set; } = default!;

        [Required]
        [Column("token"), MaxLength(256)]
        public string Token { get; set; } = default!;

        [Column("ngayTao")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("hetHanLuc")]
        public DateTime ExpiresAt { get; set; }

        [Column("daSuDung")]
        public bool Used { get; set; } = false;
    }
}
