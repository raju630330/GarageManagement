using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IStockMovementRepository
    {
        Task AddRangeAsync(IEnumerable<StockMovement> movements);
    }
}
