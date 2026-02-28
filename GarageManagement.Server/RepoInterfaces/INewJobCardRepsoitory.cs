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
        Task<BaseResultDto> SaveTyreBatteryAsync(int jobCardId, List<TyreBatteryDto> items);
        Task<BaseResultDto> SaveCancelledInvoicesAsync(int jobCardId, List<CancelledInvoiceDto> invoices);
        Task<BaseResultDto> SaveCollectionsAsync(int jobCardId, List<CollectionDto> collections);
        Task<BaseResultDto> SaveServiceSuggestionAsync(int jobCardId, string suggestion);
        Task<BaseResultDto> SaveRemarksAsync(int jobCardId, string remarks);
        Task<BaseResultDto> CompleteJobCard(JobCardBillingDto dto);
    }
}
