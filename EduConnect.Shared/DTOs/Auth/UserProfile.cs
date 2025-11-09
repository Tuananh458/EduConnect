namespace EduConnect.Shared.DTOs.Auth
{
    public class UserProfileDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public string? Email { get; set; }
        public string Role { get; set; } = "";
        public string? Avatar { get; set; }
    }

    public class UpdateProfileDto
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Avatar { get; set; }
    }
}
