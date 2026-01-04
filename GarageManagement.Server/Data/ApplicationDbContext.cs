using GarageManagement.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ------------------- DbSets -------------------
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<WorkshopProfile> WorkshopProfiles { get; set; }
        public DbSet<WorkshopUser> WorkshopUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<BookAppointment> BookAppointments { get; set; }
        public DbSet<RepairOrder> RepairOrders { get; set; }
        public DbSet<LabourDetail> LabourDetails { get; set; }
        public DbSet<TechnicianMC> TechnicianMCForms { get; set; }
        public DbSet<CheckItemEntity> CheckItems { get; set; }
        public DbSet<InventoryForm> InventoryForms { get; set; }
        public DbSet<InventoryAccessory> InventoryAccessories { get; set; }
        public DbSet<SparePartsIssueDetail> SparePartsIssueDetails { get; set; }
        public DbSet<JobCard> JobCards { get; set; }
        public DbSet<JobCardConcern> JobCardConcerns { get; set; }
        public DbSet<JobCardAdvancePayment> JobCardAdvancePayments { get; set; }
        public DbSet<AdditionalJobObserveDetail> AdditionalJobObserveDetails { get; set; }
        public DbSet<ToBeFilledBySupervisor> ToBeFilledBySupervisors { get; set; }
        public DbSet<JobCardEstimationItem> JobCardEstimationItems { get; set; }
        public DbSet<JobCardTyreBattery> JobCardTyreBatteries { get; set; }
        public DbSet<JobCardCancelledInvoice> JobCardCancelledInvoices { get; set; }
        public DbSet<JobCardCollection> JobCardCollections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------------- Singular table names -------------------
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermission");
            modelBuilder.Entity<WorkshopProfile>().ToTable("WorkshopProfile");
            modelBuilder.Entity<WorkshopUser>().ToTable("WorkshopUser");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            modelBuilder.Entity<BookAppointment>().ToTable("BookAppointment");
            modelBuilder.Entity<RepairOrder>().ToTable("RepairOrder");
            modelBuilder.Entity<LabourDetail>().ToTable("LabourDetail");
            modelBuilder.Entity<TechnicianMC>().ToTable("TechnicianMC");
            modelBuilder.Entity<CheckItemEntity>().ToTable("CheckItem");
            modelBuilder.Entity<InventoryForm>().ToTable("InventoryForm");
            modelBuilder.Entity<InventoryAccessory>().ToTable("InventoryAccessory");
            modelBuilder.Entity<SparePartsIssueDetail>().ToTable("SparePartsIssueDetail");
            modelBuilder.Entity<JobCard>().ToTable("JobCard");
            modelBuilder.Entity<JobCardConcern>().ToTable("JobCardConcern");
            modelBuilder.Entity<JobCardAdvancePayment>().ToTable("JobCardAdvancePayment");
            modelBuilder.Entity<AdditionalJobObserveDetail>().ToTable("AdditionalJobObserveDetail");
            modelBuilder.Entity<ToBeFilledBySupervisor>().ToTable("ToBeFilledBySupervisor");
            modelBuilder.Entity<JobCardEstimationItem>().ToTable("JobCardEstimationItem");
            modelBuilder.Entity<JobCardTyreBattery>().ToTable("JobCardTyreBattery");
            modelBuilder.Entity<JobCardCancelledInvoice>().ToTable("JobCardCancelledInvoice");
            modelBuilder.Entity<JobCardCollection>().ToTable("JobCardCollection");

            // ------------------- Primary keys -------------------
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Permission>().HasKey(r => r.Id);
            modelBuilder.Entity<RolePermission>().HasKey(r => r.Id);
            modelBuilder.Entity<WorkshopProfile>().HasKey(w => w.Id);
            modelBuilder.Entity<WorkshopUser>().HasKey(wu => wu.Id);
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            modelBuilder.Entity<Vehicle>().HasKey(v => v.Id);
            modelBuilder.Entity<BookAppointment>().HasKey(b => b.Id);
            modelBuilder.Entity<RepairOrder>().HasKey(r => r.Id);
            modelBuilder.Entity<LabourDetail>().HasKey(l => l.Id);
            modelBuilder.Entity<TechnicianMC>().HasKey(t => t.Id);
            modelBuilder.Entity<CheckItemEntity>().HasKey(c => c.Id);
            modelBuilder.Entity<InventoryForm>().HasKey(i => i.Id);
            modelBuilder.Entity<InventoryAccessory>().HasKey(i => i.Id);
            modelBuilder.Entity<SparePartsIssueDetail>().HasKey(s => s.Id);
            modelBuilder.Entity<JobCard>().HasKey(j => j.Id);
            modelBuilder.Entity<JobCardConcern>().HasKey(j => j.Id);
            modelBuilder.Entity<JobCardAdvancePayment>().HasKey(j => j.Id);
            modelBuilder.Entity<AdditionalJobObserveDetail>().HasKey(a => a.Id);
            modelBuilder.Entity<ToBeFilledBySupervisor>().HasKey(t => t.Id);
            modelBuilder.Entity<JobCardEstimationItem>().HasKey(j => j.Id);
            modelBuilder.Entity<JobCardTyreBattery>().HasKey(j => j.Id);
            modelBuilder.Entity<JobCardCancelledInvoice>().HasKey(j => j.Id);
            modelBuilder.Entity<JobCardCollection>().HasKey(j => j.Id);

            // ------------------- Relationships -------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<WorkshopUser>()
                .HasOne(wu => wu.User)
                .WithMany()
                .HasForeignKey(wu => wu.UserId);

            // RolePermission: Role -> RolePermissions (one-to-many)
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Delete all RolePermissions if Role is deleted

            // RolePermission: Permission -> RolePermissions (one-to-many)
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade); // Delete all RolePermissions if Permission is deleted

            // Optional: Role Name Unique
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // Optional: Permission Name Unique
            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<WorkshopUser>()
                .HasOne(wu => wu.Workshop)
                .WithMany()
                .HasForeignKey(wu => wu.WorkshopId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Customer)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookAppointment>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookAppointment>()
                .HasOne(b => b.Workshop)
                .WithMany(w => w.Appointments)
                .HasForeignKey(b => b.WorkshopId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookAppointment>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<TechnicianMC>()
                .HasMany(t => t.CheckList)
                .WithOne(c => c.TechnicianMC)
                .HasForeignKey(c => c.TechnicianMCId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.Concerns)
                .WithOne(c => c.JobCard)
                .HasForeignKey(c => c.JobCardId);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.TyreBatteries)
                .WithOne(t => t.JobCard)
                .HasForeignKey(t => t.JobCardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.CancelledInvoices)
                .WithOne(c => c.JobCard)
                .HasForeignKey(c => c.JobCardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.Collections)
                .WithOne(c => c.JobCard)
                .HasForeignKey(c => c.JobCardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobCard>()
                .HasOne(j => j.AdvancePayment)
                .WithOne(a => a.JobCard)
                .HasForeignKey<JobCardAdvancePayment>(a => a.JobCardId);

            // ------------------- Decimal types -------------------
            modelBuilder.Entity<JobCard>(entity =>
            {
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Paid).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<JobCardAdvancePayment>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Cash).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<JobCardEstimationItem>(entity =>
            {
                entity.Property(e => e.Rate).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TaxPercent).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<JobCardCancelledInvoice>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<JobCardCollection>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            });
        }
    }
}
