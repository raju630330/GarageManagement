using GarageManagement.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Data
{
        public class ApplicationDbContext : DbContext
        {
        internal readonly object ToBeFilledBySupervisors;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<WorkshopProfile> WorkshopProfiles { get; set; }

            public DbSet<BookAppointment> bookAppointments { get; set; }
            public DbSet<User> Users { get; set; }
            
           public DbSet<RepairOrder> RepairOrders {  get; set; }
           public DbSet<LabourDetail> LabourDetails { get; set; }
           public DbSet<TechnicianMC> TechnicianMCForms { get; set; }
           public DbSet<CheckItemEntity> CheckItems { get; set; }
           public DbSet<ToBeFilledBySupervisor> ToBeFilledBySupervisor{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
             base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TechnicianMC>().ToTable("TechnicianMCForms")
                .HasMany(t => t.CheckList)
                .WithOne(c => c.TechnicianMC)
                .HasForeignKey(c => c.TechnicianMCId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdditionalJobObserveDetail>().ToTable("AdditionalJobObserveDetail");

            modelBuilder.Entity<JobCard>()
           .HasMany(j => j.Concerns)
           .WithOne(c => c.JobCard)
           .HasForeignKey(c => c.JobCardId);

            modelBuilder.Entity<JobCard>()
                .HasOne(j => j.AdvancePayment)
                .WithOne(a => a.JobCard)
                .HasForeignKey<JobCardAdvancePayment>(a => a.JobCardId);

            modelBuilder.Entity<JobCardAdvancePayment>(entity =>
            {
                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)");
                entity.Property(e => e.Cash)
                      .HasColumnType("decimal(18,2)");
            });
        }
           public DbSet<InventoryForm> InventoryForms { get; set; }
           public DbSet<InventoryAccessories> InventoryAccessories { get; set; }
           public DbSet<SparePartsIssueDetail> SparePartsIssueDetails { get; set; }
           public DbSet<AdditionalJobObserveDetail> AdditionalJobObserveDetail { get; set; }

           public DbSet<JobCard> JobCards { get; set; }
           public DbSet<JobCardConcern> JobCardConcerns { get; set; }
           public DbSet<JobCardAdvancePayment> JobCardAdvancePayments { get; set; }
    }

}
