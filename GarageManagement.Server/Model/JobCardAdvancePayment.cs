using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class JobCardAdvancePayment
    {
        public long Id { get; set; }
        public long JobCardId { get; set; }

        public decimal? Cash { get; set; }
        public string? BankName { get; set; }
        public string? ChequeNo { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }

        public JobCard? JobCard { get; set; }
    }
}
