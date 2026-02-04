using GarageManagement.Server.dtos;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface INewJobCardRepsoitory
    {
        Task<List<JobCardListDto>> GetJobCards();
        Task<BaseResultDto> SaveJobCard(JobCardDto dto);
        Task<JobCardDto> GetJobCard(long id);
        Task<GetEstimationDto> GetEstimationDetails(long jobCardId);
        Task<BaseResultDto> SaveEstimationDetails(EstimationItemsSaveDto estimationItem);
    }
}
