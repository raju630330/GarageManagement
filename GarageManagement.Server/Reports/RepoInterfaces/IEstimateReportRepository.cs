using System.Data;

namespace GarageManagement.Server.Reports.RepoInterfaces
{
    public interface IEstimateReportRepository
    {
        Task<DataTable> GetEstimateReportData(long jobCardId);
    }
}
