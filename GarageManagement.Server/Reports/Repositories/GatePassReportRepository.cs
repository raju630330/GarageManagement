using GarageManagement.Server.Data;
using GarageManagement.Server.Reports.Models;
using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GarageManagement.Server.Reports.Repositories
{
    public class GatePassReportRepository:IGatePassReportRepository 
    {
        private readonly ApplicationDbContext _context;
        public GatePassReportRepository(ApplicationDbContext context) { _context = context; }

        public async Task<DataTable> GetGatePassReportData(long jobCardId)
        {
            var result = await _context.JobCards
                .Where(b => b.Id == jobCardId)
                .Select(a => new GatePassReportModel
                {
                    JobCardNo = a.JobCardNo,
                    RegistrationNo = a.RegistrationNo,
                    OdometerIn = a.OdometerIn ?? 0,
                }).FirstOrDefaultAsync();


            DataTable dt = new DataTable("GatePassDT");

            dt.Columns.Add("JobCardNo");
            dt.Columns.Add("RegistrationNo");
            dt.Columns.Add("OdometerIn");

            if (result != null)
            {
                dt.Rows.Add(
                    result.JobCardNo,
                    result.RegistrationNo,
                    result.OdometerIn
                );
            }

            return dt;
        }
    }
}
