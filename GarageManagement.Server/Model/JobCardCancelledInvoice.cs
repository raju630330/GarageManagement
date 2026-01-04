using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class JobCardCancelledInvoice
    {
        public long Id { get; set; }

        public long JobCardId { get; set; }
        public JobCard JobCard { get; set; } = null!;

        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
