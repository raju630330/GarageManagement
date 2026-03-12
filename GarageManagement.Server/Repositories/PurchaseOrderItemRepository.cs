
using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GarageManagement.Server.Repositories
{
    public class PurchaseOrderItemRepository : IPurchaseOrderItemRepository
    {
        private readonly ApplicationDbContext _db;

        public PurchaseOrderItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddRangeAsync(IEnumerable<PurchaseOrderItem> items)
        {
            await _db.PurchaseOrderItems.AddRangeAsync(items);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateInwardedQtyAsync(
            long itemId,
            decimal inwardedQty,
            long stockMovementId,
            long userId)
        {
            var item = await _db.PurchaseOrderItems.FindAsync(itemId);
            if (item == null) return;

            item.InwardedQty = inwardedQty;
            item.StockMovementId = stockMovementId;
            item.ModifiedBy = userId;
            item.ModifiedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}