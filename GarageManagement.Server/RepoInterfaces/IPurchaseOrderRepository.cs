using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<IEnumerable<OrderListDto>> GetAllAsync(long workshopId, OrderFilterDto filter);
        Task<PurchaseOrder?> GetByIdAsync(long id, long workshopId);
        Task<string?> GetLatestOrderNoAsync(long workshopId);
        Task<long> CreateAsync(PurchaseOrder order);
        Task<bool> UpdateStatusAsync(long id, string status, long userId);
        Task<bool> SoftDeleteAsync(long id, long userId);
    }
}
