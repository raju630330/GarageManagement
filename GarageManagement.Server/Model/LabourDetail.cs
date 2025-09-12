namespace GarageManagement.Server.Model
{
    public class LabourDetail
    {
        public int Id { get; set; }          
        public int SerialNo { get; set; }
        public string? Description { get; set; }
        public decimal LabourCharges { get; set; }
        public decimal OutsideLabour { get; set; }
        public decimal Amount { get; set; }
    }
}
