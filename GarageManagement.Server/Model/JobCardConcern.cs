namespace GarageManagement.Server.Model
{
    public class JobCardConcern
    {
        public int Id { get; set; }
        public int JobCardId { get; set; }

        public string? Text { get; set; }
        public bool Active { get; set; }

        public JobCard? JobCard { get; set; }
    }
}
