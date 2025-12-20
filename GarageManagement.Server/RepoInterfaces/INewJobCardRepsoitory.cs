using GarageManagement.Server.dtos;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface INewJobCardRepsoitory
    {
        Task<List<JobCardListDto>> GetJobCards();
        Task<BaseResultDto> SaveJobCard(JobCardDto dto);
        Task<BaseResultDto> SaveEstimationDetails(EstimationSaveDto model);
        Task<JobCardDto> GetJobCard(long id);
        Task<GetEstimationDto> GetEstimationDetails(long jobCardId);
    }
}
