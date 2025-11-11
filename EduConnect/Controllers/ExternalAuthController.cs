using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduConnect.Models.Auth;

namespace EduConnect.Models.Auth
{
    [Table("TOKENLAMMOI")]
    public class TokenLamMoi
    {
        [Key]
        [Column("maTokenLamMoi")]
        public int MaTokenLamMoi { get; set; }

        [Required]
        [Column("maNguoiDung")]
        public Guid MaNguoiDung { get; set; }

        [ForeignKey(nameof(MaNguoiDung))]
        public NguoiDung NguoiDung { get; set; } = default!;

        [Required]
        [Column("maToken")]
        public string MaToken { get; set; } = string.Empty; // 🔁 trùng với RefreshToken.Token

        [Column("hetHanLuc")]
        public DateTime HetHanLuc { get; set; } // 🔁 trùng với RefreshToken.ExpiresAt

        [Column("biHuyLuc")]
        public DateTime? BiHuyLuc { get; set; } // 🔁 tương ứng RefreshToken.RevokedAt

        [NotMapped]
        public bool HieuLuc => BiHuyLuc == null && DateTime.UtcNow < HetHanLuc;
    }
}
