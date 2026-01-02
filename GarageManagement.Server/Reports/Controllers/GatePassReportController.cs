using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;

namespace GarageManagement.Server.Reports.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class GatePassReportController : ControllerBase
    {
        private readonly IGatePassReportRepository _reportRepo;

        public GatePassReportController(IGatePassReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        [HttpGet("gatepass/{jobCardId}")]
        public async Task<IActionResult> ExportWorkOrder(long jobCardId)
        {
            try
            {
                var pdf = await _reportRepo.GenerateGatePassPdf(jobCardId);
                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
