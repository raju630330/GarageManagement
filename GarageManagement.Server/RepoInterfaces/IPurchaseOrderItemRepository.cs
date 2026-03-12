using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IPurchaseOrderItemRepository
    {
        Task AddRangeAsync(IEnumerable<PurchaseOrderItem> items);
        Task UpdateInwardedQtyAsync(long itemId, decimal inwardedQty, long stockMovementId, long userId);
    }
}
