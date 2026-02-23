using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class WorkshopUser : BaseEntity
    {
        public long WorkshopId { get; set; }
        public WorkshopProfile? Workshop { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
