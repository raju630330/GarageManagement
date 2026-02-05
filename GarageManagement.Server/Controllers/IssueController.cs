using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    public class IssueController : BaseAuthorizationController
    {
        private readonly IIssueRepository _issueRepository; 
        public IssueController(IIssueRepository issueRepository) { _issueRepository = issueRepository; }

        [HttpGet("pending-items/{jobCardId}")]
        public async Task<IActionResult> GetPendingItems(long jobCardId)
        {
            var result = await _issueRepository.GetEstimationPendingItems(jobCardId);
            return Ok(result);
        }
        [HttpPost("issue-parts")]
        public async Task<IActionResult> IssueParts([FromBody] IssuePartsRequestDto request)
        {
            var result = await _issueRepository.IssueParts(request);
            if (result.IsSuccess == false)
            { 
                return BadRequest(result);  
            }
            return Ok(result);  
        }
        [HttpGet("issued-items/{jobCardId}")]
        public async Task<IActionResult> GetIssuedItems(long jobCardId)
        {
            var result = await _issueRepository.GetIssuedItems(jobCardId);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
