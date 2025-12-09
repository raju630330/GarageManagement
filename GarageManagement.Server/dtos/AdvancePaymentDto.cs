namespace GarageManagement.Server.dtos
{
    public class AdvancePaymentDto
    {
        public decimal? Cash { get; set; }
        public string? BankName { get; set; }
        public string? ChequeNo { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}
