using System;
using System.Collections.Generic;

namespace EduConnect.Models.Auth
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Student"; // Admin / Teacher / Student / Parent
        public string AuthProvider { get; set; } = "Local"; // Local / Google
        public bool MustChangePassword { get; set; } = true;
        public byte Status { get; set; } = 1; // 1=Active, 0=Locked
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // 1 User có thể có nhiều RefreshTokens
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
