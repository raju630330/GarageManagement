using System.Data;

namespace GarageManagement.Server.Reports.RepoInterfaces
{
    public interface IGatePassReportRepository
    {
        Task<byte[]> GenerateGatePassPdf(long jobCardId);
    }
}
