using System.Data;

namespace GarageManagement.Server.Reports.RepoInterfaces
{
    public interface IEstimateReportRepository
    {
        Task<byte[]> GenerateEstimatePdf(long jobCardId);
    }
}
