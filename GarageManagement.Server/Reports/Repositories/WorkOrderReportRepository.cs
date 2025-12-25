using GarageManagement.Server.Data;
using GarageManagement.Server.Reports.Models;
using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GarageManagement.Server.Reports.Repositories
{
    public class WorkOrderReportRepository:IWorkOrderReportRepository
    {
        private readonly ApplicationDbContext _context;
        public WorkOrderReportRepository(ApplicationDbContext context) { _context = context; }

        public async Task<DataTable> GetWorkOrderReportData(long jobCardId)
        {
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
            dt.Columns.Add("IsApproved", typeof(bool)); // ✅ REQUIRED

            if (result != null)
            {
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
            }

            return dt;
        }


    }
}
