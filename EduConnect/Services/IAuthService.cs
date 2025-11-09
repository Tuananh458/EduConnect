using EduConnect.Models;
using EduConnect.Shared.DTOs.Auth;

namespace EduConnect.Services
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> RegisterAsync(string username, string fullName, string email, string password);
        Task<(string accessToken, string refreshToken)> LoginAsync(string emailOrUsername, string password);
        Task<UserProfileDto?> GetProfileAsync(Guid userId); 
        Task UpdateProfileAsync(Guid userId, UpdateProfileDto dto); 
    }
}
