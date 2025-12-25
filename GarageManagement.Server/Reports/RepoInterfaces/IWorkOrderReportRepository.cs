using System.Data;

namespace GarageManagement.Server.Reports.RepoInterfaces
{
    public interface IWorkOrderReportRepository
    {
        Task<DataTable> GetWorkOrderReportData(long jobCardId);
    }
}
