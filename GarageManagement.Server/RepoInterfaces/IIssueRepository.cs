using GarageManagement.Server.dtos;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IIssueRepository
    {
        Task<BaseResultDto<List<PendingIssueItemDto>>> GetEstimationPendingItems(long jobCardId);
        Task<BaseResultDto> IssueParts(IssuePartsRequestDto request);
        Task<BaseResultDto<List<IssuedItemDto>>> GetIssuedItems(long jobCardId);
        Task<BaseResultDto> ReturnIssuedParts(ReturnPartsRequestDto request);
        Task<BaseResultDto<List<ReturnItemDto>>> GetReturnItems(long jobCardId);
    }
}
