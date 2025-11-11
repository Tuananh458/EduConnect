using EduConnect.Models;

namespace EduConnect.Services
{
    public interface ITokenService
    {
        (string accessToken, DateTime expiresAt) CreateAccessToken(NguoiDung nguoiDung);
        string CreateRefreshToken();
    }
}
