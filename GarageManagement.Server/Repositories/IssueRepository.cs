using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Helpers;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Security.Claims;
namespace GarageManagement.Server.Repositories
{
    public class IssueRepository:IIssueRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtUserContext _jwtUser;
        private readonly IHelperRepository _helperRepository;
        public IssueRepository(ApplicationDbContext context, IJwtUserContext jwtUser,IHelperRepository helperRepository) { _context = context; _jwtUser = jwtUser; _helperRepository = helperRepository; }

        public async Task<BaseResultDto<List<PendingIssueItemDto>>> GetEstimationPendingItems(long jobCardId)
        {
            var items = await _context.JobCardEstimationItems
                .Include(x => x.Part).ThenInclude(p => p.StockMovements)
                .Where(x =>
                            x.JobCardId == jobCardId &&
                            x.Type == "Part" &&
                            x.Part != null &&
                            (x.RequestedQuantity - x.IssuedQty) > 0
                      )
                .Select(x => new PendingIssueItemDto
                {
                    EstimationItemId = x.Id,
                    PartNo = x.Part.PartNo,
                    PartName = x.Part.PartName,
                    Brand = x.Part.Brand,
                    QtyOnHand = x.Part.StockMovements.Sum(sm => sm.Quantity), // calculate current stock
                    RequestedQty = x.RequestedQuantity,
                    IssuedQty = x.IssuedQty,
                    PendingQty = x.RequestedQuantity - x.IssuedQty,
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
        public async Task<BaseResultDto> IssueParts(IssuePartsRequestDto request)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "No items received"
                };
            }

            var requestDict = request.Items
                .Where(x => x.IssueQty > 0)
                .ToDictionary(x => x.EstimationItemId, x => x);

            var estimationIds = requestDict.Keys.ToList();

            var estimationItems = await _context.JobCardEstimationItems
                .Include(x => x.Part)
                .Where(x =>
                    estimationIds.Contains(x.Id) &&
                    x.JobCardId == request.JobCardId
                )
                .ToListAsync();

            if (!estimationItems.Any())
            {
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "No matching estimation items found"
                };
            }

            foreach (var estimation in estimationItems)
            {
                var req = requestDict[estimation.Id];

                var pendingQty = estimation.RequestedQuantity - estimation.IssuedQty;

                if (req.IssueQty > pendingQty)
                {
                    return new BaseResultDto
                    {
                        IsSuccess = false,
                        Message = $"Issue Qty exceeds pending qty for part {estimation.Part?.PartName}"
                    };
                }

                // 🔹 Check available stock
                var availableStock = await _context.StockMovements
                    .Where(x => x.PartId == estimation.PartId)
                    .SumAsync(x => x.Quantity);

                if (availableStock < req.IssueQty)
                {
                    return new BaseResultDto
                    {
                        IsSuccess = false,
                        Message = $"Insufficient stock for part {estimation.Part?.PartName}"
                    };
                }

                // 🔹 Update estimation
                estimation.IssuedQty += req.IssueQty;

                if (estimation.IssuedQty >= estimation.RequestedQuantity)
                {
                    estimation.ApprovalStatus = "Issued";
                    estimation.IssuedDate = DateTime.Now;
                    estimation.IssuedBy = _jwtUser.GetUserId();
                    estimation.IssuedId = "SRT-IS" + estimation.Id.ToString("D5");
                }
                else
                {
                    estimation.ApprovalStatus = "PartiallyIssued";
                    estimation.IssuedDate = DateTime.Now;
                    estimation.IssuedBy = _jwtUser.GetUserId();
                    estimation.IssuedId = "SRT-IS" + estimation.Id.ToString("D5");
                }

                // 🔹 Stock movement (ISSUE = NEGATIVE)
                _context.StockMovements.Add(new StockMovement
                {
                    PartId = estimation.PartId!.Value,
                    EstimationItemId = estimation.Id,   
                    Quantity = -req.IssueQty,
                    SellingPrice = estimation.Rate,
                    TransactionType = "ISSUE",
                    TransactionDate = DateTime.Now,
                    UserId = _helperRepository.GetUserId(), 

                });
            }

            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                IsSuccess = true,
                Message = "Parts issued successfully"
            };
        }

        public async Task<BaseResultDto<List<IssuedItemDto>>> GetIssuedItems(long jobCardId)
        {
            var items = await _context.JobCardEstimationItems
                .Include(x => x.Part)
                .Where(x => x.JobCardId == jobCardId &&
                            (x.IssuedQty - _context.StockMovements
                                .Where(sm => sm.EstimationItemId == x.Id && sm.TransactionType == "RETURN")
                                .Sum(sm => sm.Quantity)) > 0)
                .OrderByDescending(x => x.IssuedDate)
                .Select(x => new IssuedItemDto
                {
                    EstimationItemId = x.Id,
                    PartNo = x.Part.PartNo,
                    PartName = x.Part.PartName,
                    Brand = x.Part.Brand,
                    RequestedQty = x.RequestedQuantity,
                    IssuedQty = x.IssuedQty,
                    ReturnQty = _context.StockMovements
                                        .Where(sm => sm.EstimationItemId == x.Id && sm.TransactionType == "RETURN")
                                        .Sum(sm => sm.Quantity),
                    UnitPrice = x.Rate,
                    IssuedDate = x.IssuedDate,
                    IssuedTo = "",
                    IssuedId = x.IssuedId
                })
                .ToListAsync();

            return new BaseResultDto<List<IssuedItemDto>>
            {
                IsSuccess = true,
                Message = items.Any() ? "Issued items fetched successfully" : "No issued items found",
                Data = items
            };
        }


        public async Task<BaseResultDto> ReturnIssuedParts(ReturnPartsRequestDto request)
        {
            if (request.Items == null || !request.Items.Any())
                return new BaseResultDto { IsSuccess = false, Message = "No items received" };

            var estimationItems = await _context.JobCardEstimationItems
                .Include(x => x.Part)
                .Where(x => request.Items.Select(i => i.EstimationItemId).Contains(x.Id))
                .ToListAsync();

            if (!estimationItems.Any())
            {
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "No matching estimation items found"
                };
            }

            foreach (var item in estimationItems)
            {
                var returnItem = request.Items.First(i => i.EstimationItemId == item.Id);

                // 🔹 Calculate remaining issued quantity
                var totalReturnedQty = _context.StockMovements
                                        .Where(sm => sm.EstimationItemId == item.Id && sm.TransactionType == "RETURN")
                                        .Sum(sm => sm.Quantity);

                var remainingIssuedQty = item.IssuedQty - totalReturnedQty;

                if (returnItem.ReturnQty > remainingIssuedQty)
                    return new BaseResultDto
                    {
                        IsSuccess = false,
                        Message = $"Return Qty cannot exceed remaining issued qty ({remainingIssuedQty}) for {item.Part.PartName}"
                    };

                item.ApprovalStatus = "Returned";

                // Add stock movement (RETURN = positive)
                _context.StockMovements.Add(new StockMovement
                {
                    PartId = item.PartId.Value,
                    EstimationItemId = item.Id,
                    Quantity = returnItem.ReturnQty,
                    TransactionType = "RETURN",
                    SellingPrice = item.Rate,
                    TransactionDate = DateTime.Now,
                    UserId = _helperRepository.GetUserId(),
                });
            }

            await _context.SaveChangesAsync();
            return new BaseResultDto { IsSuccess = true, Message = "Parts returned successfully" };
        }

        public async Task<BaseResultDto<List<ReturnItemDto>>> GetReturnItems(long jobCardId)
        {
            var items = await _context.JobCardEstimationItems
                .Include(x => x.Part)

                // 🔗 join with RETURN stock movements
                .GroupJoin(
                    _context.StockMovements
                        .Where(sm => sm.TransactionType == "RETURN"),
                    est => est.Id,
                    sm => sm.EstimationItemId,
                    (est, returns) => new { est, returns }
                )

                // ✅ only items that actually have returns
                .Where(x =>
                    x.est.JobCardId == jobCardId &&
                    x.est.IssuedQty > 0 &&
                    x.returns.Any()
                )

                .Select(x => new ReturnItemDto
                {
                    EstimationItemId = x.est.Id,
                    PartName = x.est.Part.PartName,
                    PartNo = x.est.Part.PartNo,
                    Brand = x.est.Part.Brand,

                    RequestedQty = x.est.RequestedQuantity,
                    IssuedQty = x.est.IssuedQty,

                    ReturnQty = x.returns.Sum(r => r.Quantity),

                    ReturnDate = x.returns
                        .OrderByDescending(r => r.TransactionDate)
                        .Select(r => r.TransactionDate)
                        .FirstOrDefault(),

                    UnitPrice = x.est.Rate,
                    IssuedDate = x.est.IssuedDate,
                    IssuedTo = ""
                })
                .ToListAsync();

            return new BaseResultDto<List<ReturnItemDto>>
            {
                IsSuccess = true,
                Data = items
            };
        }


    }
}
