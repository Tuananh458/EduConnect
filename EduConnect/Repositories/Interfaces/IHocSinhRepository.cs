using EduConnect.Models;

namespace EduConnect.Repositories.Interfaces
{
    public interface IHocSinhRepository
    {
        Task<List<HocSinh>> GetByLopAsync(int maLopHoc);
        Task<HocSinh?> GetByIdAsync(int maHocSinh);
        Task AddAsync(HocSinh hocSinh);
        Task UpdateAsync(HocSinh hocSinh);

        Task DeleteAsync(int maHocSinh);
        Task SaveChangesAsync();
        Task DeleteManyAsync(List<int> ids);

    }
}
