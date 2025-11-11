using EduConnect.Shared.DTOs.HocLieu;

namespace EduConnect.Services.Interfaces
{
    public interface IBaiLamHocLieuService
    {
        Task<DeThiHocLieuDto?> GetDeThiAsync(int hocLieuId);
        Task<int> TaoBaiLamAsync(TaoBaiLamRequest dto);
        Task<bool> NopBaiAsync(NopBaiRequest dto);
        Task<KetQuaBaiLamDto?> GetChiTietAsync(int baiLamId);
        Task<IEnumerable<BaiLamHocLieuDto>> GetKetQuaTheoHocLieuAsync(int hocLieuId);
    }
}
