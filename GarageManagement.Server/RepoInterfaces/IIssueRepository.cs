using GarageManagement.Server.dtos;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IIssueRepository
    {
        Task<BaseResultDto<List<PendingIssueItemDto>>> GetEstimationPendingItems(long jobCardId);
    }
}
