using GarageManagement.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Data
{
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<WorkshopProfile> WorkshopProfiles { get; set; }

            public DbSet<BookAppointment> bookAppointments { get; set; }
            public DbSet<User> Users { get; set; }
            
        public DbSet<RepairOrder> RepairOrders {  get; set; }
        public DbSet<LabourDetail> LabourDetails { get; set; }

    }

}
