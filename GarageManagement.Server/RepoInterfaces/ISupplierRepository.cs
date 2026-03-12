using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<SupplierDto>> GetAllActiveAsync(long workshopId);
        Task<bool> ExistsAsync(long id, long workshopId);
        Task<long> CreateAsync(Supplier supplier);
    }
}
