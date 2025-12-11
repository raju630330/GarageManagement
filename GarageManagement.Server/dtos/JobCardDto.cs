namespace GarageManagement.Server.dtos
{
    public class JobCardDto
    {
        public  long Id { get; set; }
        public VehicleDataDto VehicleData { get; set; }
        public List<ConcernDto> Concerns { get; set; }
        public CustomerInfoDto CustomerInfo { get; set; }
        public AdvancePaymentDto AdvancePayment { get; set; }
    }
}
