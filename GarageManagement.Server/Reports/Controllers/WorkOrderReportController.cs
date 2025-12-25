using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;

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
                var dataTable = await _reportRepo.GetWorkOrderReportData(jobCardId);

                if (dataTable == null || dataTable.Rows.Count == 0)
                    return NotFound("No data found");

                LocalReport report = new LocalReport();

                report.ReportPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Reports", "RDLC", "design.rdlc");

                report.EnableExternalImages = true;

                report.DataSources.Add(
                    new ReportDataSource("WorkOrderDS", dataTable));

                var pdf = report.Render("PDF");

                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                // THIS WILL SHOW REAL ERROR IN ANGULAR
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
