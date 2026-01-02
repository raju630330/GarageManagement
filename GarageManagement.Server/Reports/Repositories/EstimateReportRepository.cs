using GarageManagement.Server.Data;
using GarageManagement.Server.Reports.Models;
using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using System.Data;

namespace GarageManagement.Server.Reports.Repositories
{
    public class EstimateReportRepository:IEstimateReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public EstimateReportRepository(ApplicationDbContext dbContext) { _dbContext = dbContext; }

        public async Task<byte[]> GenerateEstimatePdf(long jobCardId)
        {
            // 1️⃣ Fetch data
            var result = await _dbContext.JobCards
                .Where(b => b.Id == jobCardId)
                .Select(a => new EstimateReportModel
                {
                    TicketNo = a.JobCardNo,
                    VehicleNo = a.RegistrationNo,
                    VINNo = a.Vin,
                    EngineNo = a.EngineNo,
                    ServiceType = a.ServiceType,
                    AdvisorName = a.ServiceAdvisor,
                    Technician = a.Technician,
                    Vendor = a.Vendor,
                    Date = DateTime.Now
                })
                .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("No Job Card found");

            // 2️⃣ Create DataTable
            DataTable dt = new DataTable("EstimateDT");

            dt.Columns.Add("TicketNo");
            dt.Columns.Add("VehicleNo");
            dt.Columns.Add("VINNo");
            dt.Columns.Add("EngineNo");
            dt.Columns.Add("ServiceType");
            dt.Columns.Add("AdvisorName");
            dt.Columns.Add("Technician");
            dt.Columns.Add("Vendor");
            dt.Columns.Add("Date");

            dt.Rows.Add(
                result.TicketNo,
                result.VehicleNo,
                result.VINNo,
                result.EngineNo,
                result.ServiceType,
                result.AdvisorName,
                result.Technician,
                result.Vendor,
                result.Date
            );

            // 3️⃣ Create Report
            LocalReport report = new LocalReport();

            report.ReportPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Reports", "RDLC", "EstimateReportDesigner.rdlc");

            report.EnableExternalImages = true;

            // 4️⃣ Add DataSource
            report.DataSources.Clear();
            report.DataSources.Add(
                new ReportDataSource("EstimateDS", dt)
            );

            // 5️⃣ REPORT PARAMETERS (MUST MATCH RDLC)
            var parameters = new List<ReportParameter>
            {
                new ReportParameter("TicketNo", result.TicketNo ?? ""),
                new ReportParameter("VehicleNo", result.VehicleNo ?? ""),
                new ReportParameter("VINNo", result.VINNo ?? ""),
                new ReportParameter("EngineNo", result.EngineNo ?? ""),
                new ReportParameter("ServiceType", result.ServiceType ?? ""),
                new ReportParameter("AdvisorName", result.AdvisorName ?? ""),
                new ReportParameter("Technician", result.Technician ?? ""),
                new ReportParameter("Vendor", result.Vendor ?? ""),
                new ReportParameter("Date", result.Date.ToString("dd-MM-yyyy"))
            };

            report.SetParameters(parameters);

            // 6️⃣ Render PDF
            return report.Render("PDF");
        }
    }
}
