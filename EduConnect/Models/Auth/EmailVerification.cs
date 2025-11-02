using System.ComponentModel.DataAnnotations;

namespace EduConnect.Models.Auth
{
    public class EmailVerification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Quan hệ
        public User User { get; set; } = default!;
    }
}
