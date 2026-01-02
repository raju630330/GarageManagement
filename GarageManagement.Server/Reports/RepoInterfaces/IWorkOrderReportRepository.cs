using System.Data;

namespace GarageManagement.Server.Reports.RepoInterfaces
{
    public interface IWorkOrderReportRepository
    {
        Task<byte[]> GenerateWorkOrderPdf(long jobCardId);
    }
}
