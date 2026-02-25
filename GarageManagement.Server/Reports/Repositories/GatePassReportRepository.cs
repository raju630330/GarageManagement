using GarageManagement.Server.Data;
using GarageManagement.Server.Reports.Models;
using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using System.Data;

namespace GarageManagement.Server.Reports.Repositories
{
    public class GatePassReportRepository:IGatePassReportRepository 
    {
        private readonly ApplicationDbContext _context;
        public GatePassReportRepository(ApplicationDbContext context) { _context = context; }

        public async Task<byte[]> GenerateGatePassPdf(long jobCardId)
        {
            // 1️⃣ Fetch data
            var result = await _context.JobCards
                .Where(b => b.Id == jobCardId)
                .Select(a => new GatePassReportModel
                {
                    JobCardNo = a.JobCardNo,
                    RegistrationNo = a.RegistrationNo,
                    OdometerIn = a.OdometerIn 
                })
                .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("No Job Card found");

            // 2️⃣ Create DataTable
            DataTable dt = new DataTable("GatePassDT");

            dt.Columns.Add("JobCardNo");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("OdometerIn");

            dt.Rows.Add(
                result.JobCardNo,
                result.RegistrationNo,
                result.OdometerIn
            );

            // 3️⃣ Create Report
            LocalReport report = new LocalReport();

            report.ReportPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Reports", "RDLC", "GatePassReportDesigner.rdlc");

            report.EnableExternalImages = true;

            // 4️⃣ Add DataSource
            report.DataSources.Clear();
            report.DataSources.Add(
                new ReportDataSource("GatePassDS", dt)
            );

            // 5️⃣ Report Parameters (IMPORTANT)
            var parameters = new List<ReportParameter>
            {
                new ReportParameter("JobCardNo", result.JobCardNo ?? ""),
                new ReportParameter("RegistrationNo", result.RegistrationNo ?? ""),
                new ReportParameter("OdometerIn", result.OdometerIn.ToString())
            };

            report.SetParameters(parameters);

            // 6️⃣ Render PDF
            return report.Render("PDF");
        }
    }
}
