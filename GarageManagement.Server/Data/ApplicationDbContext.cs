using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GarageManagement.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHelperRepository _helperRepository;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHelperRepository helperRepository)
            : base(options) { _helperRepository = helperRepository; }

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
        public DbSet<WorkshopAddress> WorkshopAddresses { get; set; }
        public DbSet<WorkshopTiming> WorkshopTimings { get; set; }

        public DbSet<Service> Services { get; set; }
        public DbSet<WorkshopService> WorkshopServices { get; set; }

        public DbSet<WorkshopWorkingDay> WorkshopWorkingDays { get; set; }

        public DbSet<WorkshopBusinessConfig> WorkshopBusinessConfigs { get; set; }

        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<WorkshopPaymentMode> WorkshopPaymentModes { get; set; }

        public DbSet<WorkshopMedia> WorkshopMedias { get; set; }
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
            modelBuilder.Entity<WorkshopAddress>().ToTable("WorkshopAddress");
            modelBuilder.Entity<WorkshopTiming>().ToTable("WorkshopTiming");

            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<WorkshopService>().ToTable("WorkshopService");

            modelBuilder.Entity<WorkshopWorkingDay>().ToTable("WorkshopWorkingDay");

            modelBuilder.Entity<WorkshopBusinessConfig>().ToTable("WorkshopBusinessConfig");

            modelBuilder.Entity<PaymentMode>().ToTable("PaymentMode");
            modelBuilder.Entity<WorkshopPaymentMode>().ToTable("WorkshopPaymentMode");

            modelBuilder.Entity<WorkshopMedia>().ToTable("WorkshopMedia");


        
            // ------------------- Relationships -------------------

            // User -> Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // WorkshopUser -> User & Workshop
            modelBuilder.Entity<WorkshopUser>()
                  .HasOne(wu => wu.User)
                  .WithMany(u => u.WorkshopUsers)
                  .HasForeignKey(wu => wu.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkshopUser>()
                 .HasOne(wu => wu.Workshop)
                 .WithMany(w => w.WorkshopUsers)
                 .HasForeignKey(wu => wu.WorkshopId)
                 .OnDelete(DeleteBehavior.Restrict);

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
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookAppointment>()
                .HasOne(b => b.Workshop)
                .WithMany(w => w.Appointments)
                .HasForeignKey(b => b.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookAppointment>()
                .HasOne(b => b.User)
                .WithMany(a=> a.BookAppointments)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookAppointment>()
                .HasOne(b => b.Vehicle)
                .WithMany(a=> a.Appointments)
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.NoAction);

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
               .WithOne(a=> a.RepairOrder)
               .HasForeignKey<RepairOrder>(r => r.BookingAppointmentId)
               .OnDelete(DeleteBehavior.Restrict);

            // ------------------- Decimal columns -------------------
            modelBuilder.Entity<JobCard>(entity =>
            {
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Paid).HasColumnType("decimal(18,2)");
                entity.Property(e => e.BalanceAmount).HasColumnType("decimal(18,2)");

                entity.Property(e => e.GrossAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.NetAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RoundOffAmount).HasColumnType("decimal(18,2)");

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

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.EstimationItem)
                .WithMany(e => e.StockMovements)
                .HasForeignKey(sm => sm.EstimationItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Part)
                .WithMany(p => p.StockMovements)
                .HasForeignKey(sm => sm.PartId);

            // ===============================
            // WorkshopProfile
            // ===============================

            modelBuilder.Entity<WorkshopProfile>()
                .HasMany(w => w.WorkshopUsers)
                .WithOne(wu => wu.Workshop)
                .HasForeignKey(wu => wu.WorkshopId);

            modelBuilder.Entity<WorkshopProfile>()
                .HasMany(w => w.Appointments)
                .WithOne(a => a.Workshop)
                .HasForeignKey(a => a.WorkshopId);

            modelBuilder.Entity<WorkshopProfile>()
                .HasMany(w => w.WorkshopMedias)
                .WithOne(m => m.Workshop)
                .HasForeignKey(m => m.WorkshopId);

            modelBuilder.Entity<WorkshopProfile>()
                .HasMany(w => w.WorkshopPaymentModes)
                .WithOne(pm => pm.Workshop)
                .HasForeignKey(pm => pm.WorkshopId);

            // ===============================
            // WorkshopAddress (1 : 1)
            // ===============================

            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.Address)
                .WithOne(a => a.Workshop)
                .HasForeignKey<WorkshopAddress>(a => a.WorkshopId);

            // ===============================
            // WorkshopTiming (1 : 1)
            // ===============================

            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.Timing)
                .WithOne(t => t.Workshop)
                .HasForeignKey<WorkshopTiming>(t => t.WorkshopId);

            // ===============================
            // WorkshopService (M : M)
            // ===============================

            modelBuilder.Entity<WorkshopService>()
                .HasOne(ws => ws.Workshop)
                .WithMany(w => w.Services)
                .HasForeignKey(ws => ws.WorkshopId);


            // ===============================
            // WorkshopWorkingDay (1 : M)
            // ===============================

            modelBuilder.Entity<WorkshopWorkingDay>()
                .HasOne(wd => wd.Workshop)
                .WithMany(w => w.WorkingDays)
                .HasForeignKey(wd => wd.WorkshopId);

            // ===============================
            // WorkshopBusinessConfig (1 : 1)
            // ===============================

            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.WorkshopBusinessConfigs)
                .WithOne(b => b.Workshop)
                .HasForeignKey<WorkshopBusinessConfig>(b => b.WorkshopId);

            // ===============================
            // WorkshopPaymentMode (M : M)
            // ===============================

            modelBuilder.Entity<WorkshopPaymentMode>()
                .HasOne(wp => wp.Workshop)
                .WithMany(w => w.WorkshopPaymentModes) // ✅ FIX
                .HasForeignKey(wp => wp.WorkshopId);


            // ===============================
            // WorkshopMedia (1 : M)
            // ===============================

            modelBuilder.Entity<WorkshopMedia>()
                .HasOne(m => m.Workshop)
                .WithMany(w => w.WorkshopMedias) // ✅ FIX
                .HasForeignKey(m => m.WorkshopId);

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.User)
                .WithMany(u => u.StockMovements)
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.ParentWorkshop)
                .WithMany(w => w.Branches)
                .HasForeignKey(w => w.ParentWorkshopId)
                .OnDelete(DeleteBehavior.Restrict); // 🔥 IMPORTANT

        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        // Entities where WorkshopId is a FK (relationship), NOT an audit tenant field
        private static readonly HashSet<Type> RelationshipEntities = new()
        {
            typeof(WorkshopProfile),
            typeof(WorkshopAddress),
            typeof(WorkshopTiming),
            typeof(WorkshopBusinessConfig),
            typeof(WorkshopService),
            typeof(WorkshopWorkingDay),
            typeof(WorkshopPaymentMode),
            typeof(WorkshopMedia)
        };

        private void UpdateAuditFields()
        {
            var currentUserId = _helperRepository.GetUserId();
            var currentWorkshopId = _helperRepository.GetWorkshopId();
            var currentTime = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.RowState = 1;
                        entry.Entity.ModifiedBy = currentUserId;
                        entry.Entity.ModifiedOn = currentTime;

                        // ✅ Only set WorkshopId for tenant-scoped entities
                        // ❌ Never set it for relationship entities (their WorkshopId is a FK set by EF)
                        if (!RelationshipEntities.Contains(entry.Entity.GetType())
                            && !entry.Entity.WorkshopId.HasValue
                            && currentWorkshopId > 0)
                        {
                            entry.Entity.WorkshopId = currentWorkshopId;
                        }
                        break;

                    case EntityState.Modified:
                        entry.Entity.RowState = 2;
                        entry.Entity.ModifiedBy = currentUserId;
                        entry.Entity.ModifiedOn = currentTime;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.RowState = 3;
                        entry.Entity.ModifiedBy = currentUserId;
                        entry.Entity.ModifiedOn = currentTime;
                        break;
                }
            }
        }
    }
}
