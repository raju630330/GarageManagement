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
                var dataTable = await _reportRepo.GetEstimateReportData(jobCardId);

                if (dataTable == null || dataTable.Rows.Count == 0)
                    return NotFound("No data found");

                LocalReport report = new LocalReport();

                report.ReportPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Reports", "RDLC", "EstimateReportDesigner.rdlc");

                report.EnableExternalImages = true;

                report.DataSources.Add(
                    new ReportDataSource("EstimateDS", dataTable));

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
