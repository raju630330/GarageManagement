namespace GarageManagement.Server.Model
{
    public class JobCardCollection : BaseEntity
    {
        public long JobCardId { get; set; }
        public JobCard? JobCard { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Bank { get; set; } = string.Empty;
        public string ChequeNo { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}
