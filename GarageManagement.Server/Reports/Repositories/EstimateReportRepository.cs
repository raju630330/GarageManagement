using GarageManagement.Server.Data;
using GarageManagement.Server.Reports.Models;
using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GarageManagement.Server.Reports.Repositories
{
    public class EstimateReportRepository:IEstimateReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public EstimateReportRepository(ApplicationDbContext dbContext) { _dbContext = dbContext; }

        public async Task<DataTable> GetEstimateReportData(long jobCardId)
        {
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

            DataTable dt = new DataTable("WorkOrderDT");

            dt.Columns.Add("TicketNo");
            dt.Columns.Add("VehicleNo");
            dt.Columns.Add("VINNo");
            dt.Columns.Add("EngineNo");
            dt.Columns.Add("ServiceType");
            dt.Columns.Add("AdvisorName");
            dt.Columns.Add("Technician");
            dt.Columns.Add("Vendor");
            dt.Columns.Add("Date");

            if (result != null)
            {
                dt.Rows.Add(
                    result.TicketNo,
                    result.VehicleNo,
                    result.VINNo,
                    result.EngineNo,
                    result.ServiceType,
                    result.AdvisorName,
                    result.Technician,
                    result.Vendor
                );
            }

            return dt;

        }
    }
}
