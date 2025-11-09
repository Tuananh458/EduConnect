using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Interfaces
{
    public interface IBaiLamHocLieuService
    {
        Task<double> LuuBaiLamAsync(BaiLamHocLieuRequest req);
    }
}
