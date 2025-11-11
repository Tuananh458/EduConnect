using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Models
{
    [Table("TOKENLAMMOI")]
    public class TokenLamMoi
    {
        [Key]
        [Column("maTokenLamMoi")]
        public Guid MaTokenLamMoi { get; set; } = Guid.NewGuid();

        [Required]
        [Column("maNguoiDung")]
        public Guid MaNguoiDung { get; set; }

        [ForeignKey(nameof(MaNguoiDung))]
        public NguoiDung NguoiDung { get; set; } = default!;

        [Required]
        [Column("token"), MaxLength(512)]
        public string Token { get; set; } = default!;

        [Column("ngayTao")]
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        [Column("hetHanLuc")]
        public DateTime ExpiresAt { get; set; }

        [Column("biHuyLuc")]
        public DateTime? RevokedAt { get; set; }

        [NotMapped]
        public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
    }
}
