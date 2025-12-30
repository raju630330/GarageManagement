using System.Data;

namespace GarageManagement.Server.Reports.RepoInterfaces
{
    public interface IGatePassReportRepository
    {
        Task<DataTable> GetGatePassReportData(long jobCardId);
    }
}
