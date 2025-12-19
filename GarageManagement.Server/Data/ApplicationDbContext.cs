using GarageManagement.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<WorkshopProfile> WorkshopProfiles { get; set; }
        public DbSet<BookAppointment> BookAppointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RepairOrder> RepairOrders { get; set; }
        public DbSet<LabourDetail> LabourDetails { get; set; }
        public DbSet<TechnicianMC> TechnicianMCForms { get; set; }
        public DbSet<CheckItemEntity> CheckItems { get; set; }
        public DbSet<InventoryForm> InventoryForms { get; set; }
        public DbSet<InventoryAccessories> InventoryAccessories { get; set; }
        public DbSet<SparePartsIssueDetail> SparePartsIssueDetails { get; set; }
        public DbSet<JobCard> JobCards { get; set; }
        public DbSet<JobCardConcern> JobCardConcerns { get; set; }
        public DbSet<JobCardAdvancePayment> JobCardAdvancePayments { get; set; }
        public DbSet<AdditionalJobObserveDetail> AdditionalJobObserveDetail { get; set; }
        public DbSet<ToBeFilledBySupervisor> ToBeFilledBySupervisor { get; set; }
        public DbSet<JobCardEstimationItem> JobCardEstimationItems { get; set; }
        public DbSet<JobCardTyreBattery> JobCardTyreBatteries { get; set; }
        public DbSet<JobCardCancelledInvoice> JobCardCancelledInvoices { get; set; }
        public DbSet<JobCardCollection> JobCardCollections { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TechnicianMC>()
                .ToTable("TechnicianMCForms")
                .HasMany(t => t.CheckList)
                .WithOne(c => c.TechnicianMC)
                .HasForeignKey(c => c.TechnicianMCId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.Concerns)
                .WithOne(c => c.JobCard)
                .HasForeignKey(c => c.JobCardId);

            modelBuilder.Entity<JobCard>()
                .HasOne(j => j.AdvancePayment)
                .WithOne(a => a.JobCard)
                .HasForeignKey<JobCardAdvancePayment>(a => a.JobCardId);

            // ✅ JobCard decimal fields
            modelBuilder.Entity<JobCard>(entity =>
            {
                entity.Property(e => e.Discount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Paid)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<JobCardAdvancePayment>(entity =>
            {
                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Cash)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<JobCardTyreBattery>()
                .HasOne(t => t.JobCard)
                .WithMany(j => j.TyreBatteries)
                .HasForeignKey(t => t.JobCardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCardCancelledInvoice>(entity =>
            {
                entity.HasOne(c => c.JobCard)
                      .WithMany(j => j.CancelledInvoices)
                      .HasForeignKey(c => c.JobCardId)
                      .OnDelete(DeleteBehavior.Cascade);

                // ✅ Fix warning
                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<JobCardCollection>(entity =>
            {
                entity.HasOne(c => c.JobCard)
                      .WithMany(j => j.Collections)
                      .HasForeignKey(c => c.JobCardId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)");
            });

            // ✅ JobCardEstimationItem decimal fields
            modelBuilder.Entity<JobCardEstimationItem>(entity =>
            {
                entity.Property(e => e.Rate)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Discount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TaxAmount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TaxPercent)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}
