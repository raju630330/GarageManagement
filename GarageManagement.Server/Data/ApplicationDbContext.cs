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

            // ===============================
            // TABLE NAMES
            // ===============================
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermission");
            modelBuilder.Entity<PermissionModule>().ToTable("PermissionModule");
            modelBuilder.Entity<WorkshopProfile>().ToTable("WorkshopProfile");
            modelBuilder.Entity<WorkshopUser>().ToTable("WorkshopUser");
            modelBuilder.Entity<WorkshopAddress>().ToTable("WorkshopAddress");
            modelBuilder.Entity<WorkshopTiming>().ToTable("WorkshopTiming");
            modelBuilder.Entity<WorkshopBusinessConfig>().ToTable("WorkshopBusinessConfig");
            modelBuilder.Entity<WorkshopService>().ToTable("WorkshopService");
            modelBuilder.Entity<WorkshopWorkingDay>().ToTable("WorkshopWorkingDay");
            modelBuilder.Entity<WorkshopPaymentMode>().ToTable("WorkshopPaymentMode");
            modelBuilder.Entity<WorkshopMedia>().ToTable("WorkshopMedia");
            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<PaymentMode>().ToTable("PaymentMode");
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
            modelBuilder.Entity<Part>().ToTable("Part");
            modelBuilder.Entity<StockMovement>().ToTable("StockMovement");

            // ===============================
            // GLOBAL SOFT-DELETE QUERY FILTERS
            // RowState = 3 rows are invisible in ALL queries automatically
            // ===============================
            modelBuilder.Entity<User>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<Role>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<Permission>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<PermissionModule>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopProfile>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopUser>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopAddress>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopTiming>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopBusinessConfig>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopService>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopWorkingDay>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopPaymentMode>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<WorkshopMedia>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<Service>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<PaymentMode>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<Customer>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<Vehicle>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<BookAppointment>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<RepairOrder>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<LabourDetail>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<TechnicianMC>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<CheckItemEntity>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<InventoryForm>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<InventoryAccessory>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<SparePartsIssueDetail>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<JobCard>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<JobCardConcern>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<JobCardAdvancePayment>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<AdditionalJobObserveDetail>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<ToBeFilledBySupervisor>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<JobCardEstimationItem>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<JobCardTyreBattery>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<JobCardCancelledInvoice>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<JobCardCollection>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<Part>().HasQueryFilter(x => x.RowState != 3);
            modelBuilder.Entity<StockMovement>().HasQueryFilter(x => x.RowState != 3);

            // ===============================
            // USER → ROLE
            // Restrict: can't delete Role that still has Users
            // ===============================
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // ROLE PERMISSIONS
            // NoAction: multiple parents → avoid cascade path conflict
            // soft-delete handles propagation
            // ===============================
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.PermissionModule)
                .WithMany(pm => pm.RolePermissions)
                .HasForeignKey(rp => rp.PermissionModuleId)
                .OnDelete(DeleteBehavior.NoAction);

            // ===============================
            // WORKSHOP USER
            // Restrict both sides: deleting User or Workshop
            // blocked if active WorkshopUser links exist
            // ===============================
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

            // ===============================
            // WORKSHOP SELF-REFERENCING (Parent / Branch)
            // Restrict: can't delete parent while branches exist
            // ===============================
            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.ParentWorkshop)
                .WithMany(w => w.Branches)
                .HasForeignKey(w => w.ParentWorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // WORKSHOP PROFILE CHILDREN
            // NoAction: soft-delete propagates via .Include() + RowState=3
            // ===============================
            modelBuilder.Entity<WorkshopProfile>()
                .HasMany(w => w.Appointments)
                .WithOne(a => a.Workshop)
                .HasForeignKey(a => a.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkshopProfile>()
                .HasMany(w => w.WorkshopUsers)
                .WithOne(wu => wu.Workshop)
                .HasForeignKey(wu => wu.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // WorkshopAddress (1:1)
            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.Address)
                .WithOne(a => a.Workshop)
                .HasForeignKey<WorkshopAddress>(a => a.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // WorkshopTiming (1:1)
            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.Timing)
                .WithOne(t => t.Workshop)
                .HasForeignKey<WorkshopTiming>(t => t.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // WorkshopBusinessConfig (1:1)
            modelBuilder.Entity<WorkshopProfile>()
                .HasOne(w => w.WorkshopBusinessConfigs)
                .WithOne(b => b.Workshop)
                .HasForeignKey<WorkshopBusinessConfig>(b => b.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // WorkshopService (M:M junction)
            modelBuilder.Entity<WorkshopService>()
                .HasOne(ws => ws.Workshop)
                .WithMany(w => w.Services)
                .HasForeignKey(ws => ws.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // WorkshopWorkingDay (1:M)
            modelBuilder.Entity<WorkshopWorkingDay>()
                .HasOne(wd => wd.Workshop)
                .WithMany(w => w.WorkingDays)
                .HasForeignKey(wd => wd.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // WorkshopPaymentMode (M:M junction)
            modelBuilder.Entity<WorkshopPaymentMode>()
                .HasOne(wp => wp.Workshop)
                .WithMany(w => w.WorkshopPaymentModes)
                .HasForeignKey(wp => wp.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // WorkshopMedia (1:M)
            modelBuilder.Entity<WorkshopMedia>()
                .HasOne(m => m.Workshop)
                .WithMany(w => w.WorkshopMedias)
                .HasForeignKey(m => m.WorkshopId)
                .OnDelete(DeleteBehavior.NoAction);

            // ===============================
            // CUSTOMER → VEHICLES
            // NoAction: soft-delete handles propagation
            // ===============================
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Customer)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            // ===============================
            // BOOK APPOINTMENT
            // NoAction: 4 parents → cascade path conflict on SQL Server
            // ===============================
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
                .WithMany(a => a.BookAppointments)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookAppointment>()
                .HasOne(b => b.Vehicle)
                .WithMany(a => a.Appointments)
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.NoAction);

            // ===============================
            // REPAIR ORDER → BOOK APPOINTMENT
            // Restrict: can't delete appointment if repair order exists
            // ===============================
            modelBuilder.Entity<RepairOrder>()
                .HasOne(r => r.BookAppointment)
                .WithOne(a => a.RepairOrder)
                .HasForeignKey<RepairOrder>(r => r.BookingAppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // REPAIR ORDER CHILDREN
            // NoAction: soft-delete handles propagation
            // ===============================
            modelBuilder.Entity<ToBeFilledBySupervisor>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.ToBeFilledBySupervisors)
                .HasForeignKey(x => x.RepairOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AdditionalJobObserveDetail>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.AdditionalJobObserveDetails)
                .HasForeignKey(x => x.RepairOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SparePartsIssueDetail>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.SparePartsIssueDetails)
                .HasForeignKey(x => x.RepairOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LabourDetail>()
                .HasOne(x => x.RepairOrder)
                .WithMany(x => x.LabourDetails)
                .HasForeignKey(x => x.RepairOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TechnicianMC>()
                .HasOne(x => x.RepairOrder)
                .WithOne(x => x.TechnicianMC)
                .HasForeignKey<TechnicianMC>(x => x.RepairOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            // ===============================
            // TECHNICIAN MC → CHECK ITEMS
            // NoAction: soft-delete handles propagation
            // ===============================
            modelBuilder.Entity<TechnicianMC>()
                .HasMany(t => t.CheckList)
                .WithOne(c => c.TechnicianMC)
                .HasForeignKey(c => c.TechnicianMCId)
                .OnDelete(DeleteBehavior.NoAction);

            // ===============================
            // JOB CARD CHILDREN
            // NoAction: soft-delete handles all propagation
            // ===============================
            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.Concerns)
                .WithOne(c => c.JobCard)
                .HasForeignKey(c => c.JobCardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.TyreBatteries)
                .WithOne(t => t.JobCard)
                .HasForeignKey(t => t.JobCardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.CancelledInvoices)
                .WithOne(c => c.JobCard)
                .HasForeignKey(c => c.JobCardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.Collections)
                .WithOne(c => c.JobCard)
                .HasForeignKey(c => c.JobCardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobCard>()
                .HasOne(j => j.AdvancePayment)
                .WithOne(a => a.JobCard)
                .HasForeignKey<JobCardAdvancePayment>(a => a.JobCardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobCard>()
                .HasMany(j => j.JobCardEstimationItems)
                .WithOne(e => e.JobCard)
                .HasForeignKey(e => e.JobCardId)
                .OnDelete(DeleteBehavior.NoAction);

            // ===============================
            // PART & STOCK MOVEMENT
            // Restrict: financial audit trail — must never be lost
            // ===============================
            modelBuilder.Entity<Part>()
                .HasIndex(x => x.PartNo)
                .IsUnique();

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Part)
                .WithMany(p => p.StockMovements)
                .HasForeignKey(sm => sm.PartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.EstimationItem)
                .WithMany(e => e.StockMovements)
                .HasForeignKey(sm => sm.EstimationItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.User)
                .WithMany(u => u.StockMovements)
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // DECIMAL COLUMN TYPES
            // ===============================
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

            modelBuilder.Entity<JobCardCancelledInvoice>()
                .Property(e => e.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<JobCardCollection>()
                .Property(e => e.Amount).HasColumnType("decimal(18,2)");
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

            // Collect all BaseEntity entries snapshot (Deleted entries may change state mid-loop)
            var entries = ChangeTracker.Entries<BaseEntity>().ToList();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.RowState = 1;
                        entry.Entity.ModifiedBy = currentUserId;
                        entry.Entity.ModifiedOn = currentTime;

                        // Only auto-assign WorkshopId for tenant-scoped entities
                        // Skip relationship entities — their WorkshopId is a FK managed by EF
                        if (!RelationshipEntities.Contains(entry.Entity.GetType())
                            && entry.Entity.WorkshopId is null or 0
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
                        // Intercept ALL deletes — convert to soft-delete
                        // This also catches children IF they were loaded via .Include()
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
