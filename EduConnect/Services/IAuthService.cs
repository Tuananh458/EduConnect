using EduConnect.Models;

namespace EduConnect.Services
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> RegisterAsync(string username, string fullName, string email, string password);
        Task<(string accessToken, string refreshToken)> LoginAsync(string emailOrUsername, string password);
    }
}
