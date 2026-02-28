namespace GarageManagement.Server.Model
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public long? WorkshopId { get; set; }
        public byte RowState { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
