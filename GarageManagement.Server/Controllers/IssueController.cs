using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
