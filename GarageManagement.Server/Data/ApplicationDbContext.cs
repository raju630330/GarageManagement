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
           public DbSet<TechnicianMC> TechnicianMCForms { get; set; }
           public DbSet<CheckItemEntity> CheckItems { get; set; }

           protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
             base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TechnicianMC>().ToTable("TechnicianMCForms")
                .HasMany(t => t.CheckList)
                .WithOne(c => c.TechnicianMC)
                .HasForeignKey(c => c.TechnicianMCId)
                .OnDelete(DeleteBehavior.Cascade);
            }
           public DbSet<InventoryForm> InventoryForms { get; set; }
           public DbSet<InventoryAccessories> InventoryAccessories { get; set; }
           public DbSet<SparePartsIssueDetail> SparePartsIssueDetails { get; set; }
           public DbSet<AdditionalJobObserveDetail> AdditionalJobObserveDetail { get; set; }
    }

}
