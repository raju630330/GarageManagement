using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GarageManagement.Server.Controllers
{
    public class NewJobCardController : BaseAuthorizationController
    {

        private readonly ApplicationDbContext _context;
        private readonly IAutoCompleteRepository _autoCompleteRepository;
        private readonly INewJobCardRepsoitory _newJobCardRepsoitory;

        public NewJobCardController(ApplicationDbContext context , IAutoCompleteRepository autoCompleteRepository,INewJobCardRepsoitory newJobCardRepsoitory)
        {
            _context = context;
            _autoCompleteRepository = autoCompleteRepository;  
            _newJobCardRepsoitory = newJobCardRepsoitory;   
        }

        [HttpPost("save-jobcard")]
        public async Task<IActionResult> SaveJobCard([FromBody] JobCardDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            var job = await _newJobCardRepsoitory.SaveJobCard(dto);

            if (!job.IsSuccess) {
                return BadRequest("Internal server error");
            }

            return Ok(new { id = job.Id });
        }

        [HttpPost("save-estimation")]
        public async Task<IActionResult> SaveEstimationDetails([FromBody] EstimationItemsSaveDto estimation)
        {
            var jobCard = await _newJobCardRepsoitory.SaveEstimationDetails(estimation);
            return Ok(jobCard);
        }

        [HttpGet("search-registration")]
        public async Task<IActionResult> SearchRegistration([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required");

            var registrations = await _autoCompleteRepository.SearchRegistration(query);    

            return Ok(registrations);
        }
        [HttpGet("get-jobcard/{id}")]
        public async Task<IActionResult> GetJobCard(long id)
        {
            if (id == 0) return BadRequest("Id Not Found");

            var jobcarddetails = await _newJobCardRepsoitory.GetJobCard(id);

            if (!jobcarddetails.IsSuccess) return BadRequest("Id Not Found");

            return Ok(jobcarddetails);
        }

        [HttpGet("get-estimation/{jobCardId}")]
        public async Task<IActionResult> GetEstimationDetails(long jobCardId)
        {
            if (jobCardId == 0) return BadRequest("Id Not Found");

            var estimationdetails = await _newJobCardRepsoitory.GetEstimationDetails(jobCardId);

            if (!estimationdetails.IsSuccess) { NotFound("Id Not Found"); };
           
            return Ok(estimationdetails);
        }

        [HttpGet("jobcards")]
        public async Task<IActionResult> GetJobCards()
        {
            var jobCards = await _newJobCardRepsoitory.GetJobCards();
            if (jobCards == null) return NotFound("No Job Cards");
            return Ok(jobCards);
        }

        [HttpGet("searchJobCradsForEstimation")]
        public async Task<IActionResult> SearchJobCradsForEstimation([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query is required");

            var registrations = await _autoCompleteRepository.SearchJobCradsForEstimation(query);   
            return Ok(registrations);
        }

        // ✅ Save Tyre Battery
        [HttpPost("save-tyre-battery/{jobCardId}")]
        public async Task<IActionResult> SaveTyreBattery(int jobCardId, [FromBody] List<TyreBatteryDto> items)
        {
            var result = await _newJobCardRepsoitory.SaveTyreBatteryAsync(jobCardId, items);
            return Ok(result);
        }

        // ✅ Save Cancelled Invoices
        [HttpPost("save-cancelled-invoices/{jobCardId}")]
        public async Task<IActionResult> SaveCancelledInvoices(int jobCardId, [FromBody] List<CancelledInvoiceDto> invoices)
        {
            var result = await _newJobCardRepsoitory.SaveCancelledInvoicesAsync(jobCardId, invoices);
            return Ok(result);
        }

        // ✅ Save Collections
        [HttpPost("save-collections/{jobCardId}")]
        public async Task<IActionResult> SaveCollections(int jobCardId, [FromBody] List<CollectionDto> collections)
        {
            var result = await _newJobCardRepsoitory.SaveCollectionsAsync(jobCardId, collections);
            return Ok(result);
        }

        // ✅ Save Service Suggestions
        [HttpPost("save-service-suggestions/{jobCardId}")]
        public async Task<IActionResult> SaveServiceSuggestion(int jobCardId, [FromBody] string suggestion)
        {
            var result = await _newJobCardRepsoitory.SaveServiceSuggestionAsync(jobCardId, suggestion);
            return Ok(result);
        }

        // ✅ Save Remarks
        [HttpPost("save-remarks/{jobCardId}")]
        public async Task<IActionResult> SaveRemarks(int jobCardId, [FromBody] string remarks)
        {
            var result = await _newJobCardRepsoitory.SaveRemarksAsync(jobCardId, remarks);
            return Ok(result);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteJobCard([FromBody] JobCardBillingDto dto)
        {
            var result = await _newJobCardRepsoitory.CompleteJobCard(dto);
            return Ok(result);
        }
    }

}







    

