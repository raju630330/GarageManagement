using GarageManagement.Server.Data;
using GarageManagement.Server.Reports.Models;
using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using System.Data;

namespace GarageManagement.Server.Reports.Repositories
{
    public class WorkOrderReportRepository:IWorkOrderReportRepository
    {
        private readonly ApplicationDbContext _context;
        public WorkOrderReportRepository(ApplicationDbContext context) { _context = context; }

        public async Task<byte[]> GenerateWorkOrderPdf(long jobCardId)
        {
            // 1️⃣ Fetch data
            var result = await _context.JobCards
                .Where(b => b.Id == jobCardId)
                .Select(a => new WorkOrderReportModel
                {
                    JobCardNo = a.JobCardNo,
                    RegistrationNo = a.RegistrationNo,
                    OdometerIn = a.OdometerIn,
                    AvgKmsPerDay = a.AvgKmsPerDay,
                    Vin = a.Vin,
                    EngineNo = a.EngineNo,
                    VehicleColor = a.VehicleColor,
                    FuelType = a.FuelType,
                    ServiceType = a.ServiceType,
                    ServiceAdvisor = a.ServiceAdvisor,
                    Technician = a.Technician,
                    Vendor = a.Vendor,
                    IsApproved = true
                })
                .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("No Job Card found");

            // 2️⃣ Create DataTable
            DataTable dt = new DataTable("WorkOrderDT");

            dt.Columns.Add("JobCardNo");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("OdometerIn");
            dt.Columns.Add("AvgKmsPerDay");
            dt.Columns.Add("Vin");
            dt.Columns.Add("EngineNo");
            dt.Columns.Add("VehicleColor");
            dt.Columns.Add("FuelType");
            dt.Columns.Add("ServiceType");
            dt.Columns.Add("ServiceAdvisor");
            dt.Columns.Add("Technician");
            dt.Columns.Add("Vendor");
            dt.Columns.Add("IsApproved", typeof(bool));

            dt.Rows.Add(
                result.JobCardNo,
                result.RegistrationNo,
                result.OdometerIn,
                result.AvgKmsPerDay,
                result.Vin,
                result.EngineNo,
                result.VehicleColor,
                result.FuelType,
                result.ServiceType,
                result.ServiceAdvisor,
                result.Technician,
                result.Vendor,
                result.IsApproved
            );

            // 3️⃣ Create Report
            LocalReport report = new LocalReport();

            report.ReportPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Reports", "RDLC", "WorkOrderReportDesigner.rdlc");

            report.EnableExternalImages = true;

            // 4️⃣ Add DataSource
            report.DataSources.Clear();
            report.DataSources.Add(
                new ReportDataSource("WorkOrderDS", dt)
            );

            // 5️⃣ Set Report Parameters
            var parameters = new List<ReportParameter>
            {
                new ReportParameter("JobCardNo", result.JobCardNo ?? ""),
                new ReportParameter("RegistrationNo", result.RegistrationNo ?? ""),
                new ReportParameter("OdometerIn", result.OdometerIn?.ToString() ?? ""),
                new ReportParameter("AvgKmsPerDay", result.AvgKmsPerDay?.ToString() ?? ""),
                new ReportParameter("Vin", result.Vin ?? ""),
                new ReportParameter("EngineNo", result.EngineNo ?? ""),
                new ReportParameter("VehicleColor", result.VehicleColor ?? ""),
                new ReportParameter("FuelType", result.FuelType ?? ""),
                new ReportParameter("ServiceType", result.ServiceType ?? ""),
                new ReportParameter("ServiceAdvisor", result.ServiceAdvisor ?? ""),
                new ReportParameter("Technician", result.Technician ?? ""),
                new ReportParameter("Vendor", result.Vendor ?? ""),
                new ReportParameter("IsApproved", result.IsApproved.ToString())
            };

            report.SetParameters(parameters);

            // 6️⃣ Render PDF
            return report.Render("PDF");
        }


    }
}
