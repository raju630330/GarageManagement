namespace GarageManagement.Server.Model
{
    public class JobCardConcern : BaseEntity
    {
        public long JobCardId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool Active { get; set; }
        public JobCard? JobCard { get; set; }
    }
}
