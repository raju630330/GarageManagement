using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace GarageManagement.Server.Repositories
{
    public class IssueRepository:IIssueRepository
    {
        private readonly ApplicationDbContext _context;
        public IssueRepository(ApplicationDbContext context) { _context = context; }

        public async Task<BaseResultDto<List<PendingIssueItemDto>>> GetEstimationPendingItems(long jobCardId)
        {
            var items = await _context.JobCardEstimationItems
                .Include(x => x.Part) // include part navigation
                .Where(x =>
                    x.JobCardId == jobCardId &&
                    x.Type == "Part" &&
                    (x.Quantity - x.IssuedQty) > 0
                )
                .Select(x => new PendingIssueItemDto
                {
                    EstimationItemId = x.Id,
                    PartNo = x.Part.PartNo,
                    PartName = x.Part.PartName,
                    Brand = x.Part.Brand,
                    QtyOnHand = x.Part.StockMovements.Sum(sm => sm.Quantity), // calculate current stock
                    RequestedQty = x.Quantity,
                    IssuedQty = x.IssuedQty,
                    PendingQty = x.Quantity - x.IssuedQty,
                    SellingPrice = x.Rate,
                    InStock = x.Part.StockMovements.Sum(sm => sm.Quantity) > 0
                })
                .ToListAsync();

            return new BaseResultDto<List<PendingIssueItemDto>>
            {
                IsSuccess = true,
                Message = items.Any()
                    ? "Pending issue items fetched successfully"
                    : "No pending items found",
                Data = items
            };
        }



    }
}
