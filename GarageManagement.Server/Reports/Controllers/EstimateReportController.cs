using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;

namespace GarageManagement.Server.Reports.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class EstimateReportController : ControllerBase
    {
        private readonly IEstimateReportRepository _reportRepo;

        public EstimateReportController(IEstimateReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        [HttpGet("estimate/{jobCardId}")]
        public async Task<IActionResult> ExportEstimateDetails(long jobCardId)
        {
            try
            {
                var pdf = await _reportRepo.GenerateEstimatePdf(jobCardId);
                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }    
}
