using GarageManagement.Server.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        public DbSet<PermissionModule> PermissionModules { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
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
            modelBuilder.Entity<PermissionModule>().ToTable("PermissionModule");
            modelBuilder.Entity<Part>().ToTable("Part");
            modelBuilder.Entity<StockMovement>().ToTable("StockMovement");

            // ------------------- Primary keys and Identity -------------------
            void SetIdentity<TEntity>() where TEntity : class
            {
                modelBuilder.Entity<TEntity>().Property("Id").ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            SetIdentity<User>();
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            SetIdentity<Role>();
            modelBuilder.Entity<Permission>().HasKey(p => p.Id);
            SetIdentity<Permission>();
            modelBuilder.Entity<RolePermission>().HasKey(rp => rp.Id);
            SetIdentity<RolePermission>();
            modelBuilder.Entity<WorkshopProfile>().HasKey(w => w.Id);
            SetIdentity<WorkshopProfile>();
            modelBuilder.Entity<WorkshopUser>().HasKey(wu => wu.Id);
            SetIdentity<WorkshopUser>();
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            SetIdentity<Customer>();
            modelBuilder.Entity<Vehicle>().HasKey(v => v.Id);
            SetIdentity<Vehicle>();
            modelBuilder.Entity<BookAppointment>().HasKey(b => b.Id);
            SetIdentity<BookAppointment>();
            modelBuilder.Entity<RepairOrder>().HasKey(r => r.Id);
            SetIdentity<RepairOrder>();
            modelBuilder.Entity<LabourDetail>().HasKey(l => l.Id);
            SetIdentity<LabourDetail>();
            modelBuilder.Entity<TechnicianMC>().HasKey(t => t.Id);
            SetIdentity<TechnicianMC>();
            modelBuilder.Entity<CheckItemEntity>().HasKey(c => c.Id);
            SetIdentity<CheckItemEntity>();
            modelBuilder.Entity<InventoryForm>().HasKey(i => i.Id);
            SetIdentity<InventoryForm>();
            modelBuilder.Entity<InventoryAccessory>().HasKey(i => i.Id);
            SetIdentity<InventoryAccessory>();
            modelBuilder.Entity<SparePartsIssueDetail>().HasKey(s => s.Id);
            SetIdentity<SparePartsIssueDetail>();
            modelBuilder.Entity<JobCard>().HasKey(j => j.Id);
            SetIdentity<JobCard>();
            modelBuilder.Entity<JobCardConcern>().HasKey(j => j.Id);
            SetIdentity<JobCardConcern>();
            modelBuilder.Entity<JobCardAdvancePayment>().HasKey(j => j.Id);
            SetIdentity<JobCardAdvancePayment>();
            modelBuilder.Entity<AdditionalJobObserveDetail>().HasKey(a => a.Id);
            SetIdentity<AdditionalJobObserveDetail>();
            modelBuilder.Entity<ToBeFilledBySupervisor>().HasKey(t => t.Id);
            SetIdentity<ToBeFilledBySupervisor>();
            modelBuilder.Entity<JobCardEstimationItem>().HasKey(j => j.Id);
            SetIdentity<JobCardEstimationItem>();
            modelBuilder.Entity<JobCardTyreBattery>().HasKey(j => j.Id);
            SetIdentity<JobCardTyreBattery>();
            modelBuilder.Entity<JobCardCancelledInvoice>().HasKey(j => j.Id);
            SetIdentity<JobCardCancelledInvoice>();
            modelBuilder.Entity<JobCardCollection>().HasKey(j => j.Id);
            SetIdentity<JobCardCollection>();
            modelBuilder.Entity<PermissionModule>().HasKey(j => j.Id);
            SetIdentity<PermissionModule>();
            modelBuilder.Entity<Part>().HasKey(j => j.Id);
            SetIdentity<Part>();
            modelBuilder.Entity<StockMovement>().HasKey(j => j.Id);
            SetIdentity<StockMovement>();
            // ------------------- Relationships -------------------

            // User -> Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // WorkshopUser -> User & Workshop
            modelBuilder.Entity<WorkshopUser>()
                .HasOne(wu => wu.User)
                .WithMany()
                .HasForeignKey(wu => wu.UserId);

            modelBuilder.Entity<WorkshopUser>()
                .HasOne(wu => wu.Workshop)
                .WithMany()
                .HasForeignKey(wu => wu.WorkshopId);

            // RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.PermissionModule)
                .WithMany(pm => pm.RolePermissions)
                .HasForeignKey(rp => rp.PermissionModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer -> Vehicles
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Customer)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // BookAppointment -> Customer, Workshop, User
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

            // TechnicianMC -> CheckItems
            modelBuilder.Entity<TechnicianMC>()
                .HasMany(t => t.CheckList)
                .WithOne(c => c.TechnicianMC)
                .HasForeignKey(c => c.TechnicianMCId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobCard -> Concerns, TyreBatteries, CancelledInvoices, Collections, AdvancePayment
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

            // JobCard -> Estimation Items
            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.JobCardEstimationItems)
                .WithOne(e => e.JobCard)
                .HasForeignKey(e => e.JobCardId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobCard -> ToBeFilledBySupervisors


            modelBuilder.Entity<ToBeFilledBySupervisor>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.ToBeFilledBySupervisors)
                .HasForeignKey(x => x.RepairOrderId);

            modelBuilder.Entity<AdditionalJobObserveDetail>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.AdditionalJobObserveDetails)
                .HasForeignKey(x => x.RepairOrderId);

            modelBuilder.Entity<SparePartsIssueDetail>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.SparePartsIssueDetails)
                .HasForeignKey(x => x.RepairOrderId);

            modelBuilder.Entity<LabourDetail>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.LabourDetails)
                .HasForeignKey(x => x.RepairOrderId);

            modelBuilder.Entity<TechnicianMC>()
                .HasOne(x => x.RepairOrder)
                .WithOne(x => x.TechnicianMC)
                .HasForeignKey<TechnicianMC>(x => x.RepairOrderId);

            modelBuilder.Entity<RepairOrder>()
               .HasOne(r => r.BookAppointment)
               .WithMany()
               .HasForeignKey(r => r.BookingAppointmentId)
               .OnDelete(DeleteBehavior.Restrict);
            // ------------------- Decimal columns -------------------
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
            modelBuilder.Entity<Part>()
                   .HasIndex(x => x.PartNo)
                   .IsUnique();

            modelBuilder.Entity<StockMovement>()
                .HasOne(x => x.Part)
                .WithMany(x => x.StockMovements)
                .HasForeignKey(x => x.PartId);
        }
    }
}
