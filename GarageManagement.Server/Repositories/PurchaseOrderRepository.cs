
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public PurchaseOrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrderListDto>> GetAllAsync(long workshopId, OrderFilterDto filter)
        {
            var query = _db.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.Items)
                .Where(o => o.WorkshopId == workshopId && o.RowState < 3)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(o => o.Status == filter.Status);

            if (!string.IsNullOrWhiteSpace(filter.RegNo))
                query = query.Where(o => o.RegNo.Contains(filter.RegNo));

            if (filter.From.HasValue)
                query = query.Where(o => o.OrderDate >= filter.From.Value);

            if (filter.To.HasValue)
                query = query.Where(o => o.OrderDate <= filter.To.Value.AddDays(1));

            return await query
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderListDto
                {
                    Id = o.Id,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate.ToString("dd-MM-yyyy"),
                    SupplierName = o.Supplier != null ? o.Supplier.Name : "",
                    RegNo = o.RegNo,
                    JobCardNo = o.JobCardNo,
                    PaymentType = o.PaymentType,
                    StockType = o.StockType,
                    OrderValue = o.Items.Sum(i => i.TotalPurchasePrice),
                    OrderedParts = o.Items.Count(i => i.RowState < 3),
                    InwardedParts = o.Items.Count(i => i.InwardedQty > 0),
                    PendingParts = o.Items.Count(i => i.InwardedQty < i.Qty && i.RowState < 3),
                    Status = o.Status
                })
                .ToListAsync();
        }

        public async Task<PurchaseOrder?> GetByIdAsync(long id, long workshopId)
        {
            return await _db.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id
                                       && o.WorkshopId == workshopId
                                       && o.RowState < 3);
        }

        public async Task<string?> GetLatestOrderNoAsync(long workshopId)
        {
            return await _db.PurchaseOrders
                .Where(o => o.WorkshopId == workshopId && o.RowState < 3)
                .OrderByDescending(o => o.Id)
                .Select(o => o.OrderNo)
                .FirstOrDefaultAsync();
        }

        public async Task<long> CreateAsync(PurchaseOrder order)
        {
            _db.PurchaseOrders.Add(order);
            await _db.SaveChangesAsync();
            return order.Id;
        }

        public async Task<bool> UpdateStatusAsync(long id, string status, long userId)
        {
            var order = await _db.PurchaseOrders.FindAsync(id);
            if (order == null) return false;

            order.Status = status;
            order.ModifiedBy = userId;
            order.ModifiedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(long id, long userId)
        {
            var order = await _db.PurchaseOrders.FindAsync(id);
            if (order == null) return false;

            order.Status = "CANCELLED";
            order.RowState = 3;
            order.ModifiedBy = userId;
            order.ModifiedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}