using EduConnect.Models.Auth;

namespace EduConnect.Services
{
    public interface ITokenService
    {
        (string accessToken, DateTime expiresAt) CreateAccessToken(User user);
        string CreateRefreshToken();
    }
}
