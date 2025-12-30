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
                var dataTable = await _reportRepo.GetGatePassReportData(jobCardId);

                if (dataTable == null || dataTable.Rows.Count == 0)
                    return NotFound("No data found");

                LocalReport report = new LocalReport();

                report.ReportPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Reports", "RDLC", "GatePassReportDesigner.rdlc");

                report.EnableExternalImages = true;

                report.DataSources.Add(
                    new ReportDataSource("GatePassDS", dataTable));

                var pdf = report.Render("PDF");

                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
