using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System.Data;

namespace GarageManagement.Server.Reports.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class WorkOrderReportController : ControllerBase
    {
        private readonly IWorkOrderReportRepository _reportRepo;

        public WorkOrderReportController(IWorkOrderReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        [HttpGet("work-order/{jobCardId}")]
        public async Task<IActionResult> ExportWorkOrder(long jobCardId)
        {
            try
            {
                var pdf = await _reportRepo.GenerateWorkOrderPdf(jobCardId);
                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
