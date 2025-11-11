using EduConnect.Shared.DTOs.Auth;

namespace EduConnect.Services
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> RegisterAsync(
            string username, string fullName, string email, string password, string role);

        Task<(string accessToken, string refreshToken)> LoginAsync(string emailOrUsername, string password);
        Task<UserProfileDto?> GetProfileAsync(Guid id);
        Task UpdateProfileAsync(Guid id, UpdateProfileDto dto);
    }
}
