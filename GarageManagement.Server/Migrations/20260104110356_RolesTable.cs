using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class RolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckItems_TechnicianMCForms_TechnicianMCId",
                table: "CheckItems");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardAdvancePayments_JobCards_JobCardId",
                table: "JobCardAdvancePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardCancelledInvoices_JobCards_JobCardId",
                table: "JobCardCancelledInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardCollections_JobCards_JobCardId",
                table: "JobCardCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardConcerns_JobCards_JobCardId",
                table: "JobCardConcerns");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardEstimationItems_JobCards_JobCardId",
                table: "JobCardEstimationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardTyreBatteries_JobCards_JobCardId",
                table: "JobCardTyreBatteries");

            migrationBuilder.DropTable(
                name: "BookAppointments");

            migrationBuilder.DropTable(
                name: "InventoryAccessories");

            migrationBuilder.DropTable(
                name: "WorkshopProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnicianMCForms",
                table: "TechnicianMCForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SparePartsIssueDetails",
                table: "SparePartsIssueDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairOrders",
                table: "RepairOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabourDetails",
                table: "LabourDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardTyreBatteries",
                table: "JobCardTyreBatteries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCards",
                table: "JobCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardEstimationItems",
                table: "JobCardEstimationItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardConcerns",
                table: "JobCardConcerns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardCollections",
                table: "JobCardCollections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardCancelledInvoices",
                table: "JobCardCancelledInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardAdvancePayments",
                table: "JobCardAdvancePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryForms",
                table: "InventoryForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckItems",
                table: "CheckItems");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "TechnicianMCForms",
                newName: "TechnicianMC");

            migrationBuilder.RenameTable(
                name: "SparePartsIssueDetails",
                newName: "SparePartsIssueDetail");

            migrationBuilder.RenameTable(
                name: "RepairOrders",
                newName: "RepairOrder");

            migrationBuilder.RenameTable(
                name: "LabourDetails",
                newName: "LabourDetail");

            migrationBuilder.RenameTable(
                name: "JobCardTyreBatteries",
                newName: "JobCardTyreBattery");

            migrationBuilder.RenameTable(
                name: "JobCards",
                newName: "JobCard");

            migrationBuilder.RenameTable(
                name: "JobCardEstimationItems",
                newName: "JobCardEstimationItem");

            migrationBuilder.RenameTable(
                name: "JobCardConcerns",
                newName: "JobCardConcern");

            migrationBuilder.RenameTable(
                name: "JobCardCollections",
                newName: "JobCardCollection");

            migrationBuilder.RenameTable(
                name: "JobCardCancelledInvoices",
                newName: "JobCardCancelledInvoice");

            migrationBuilder.RenameTable(
                name: "JobCardAdvancePayments",
                newName: "JobCardAdvancePayment");

            migrationBuilder.RenameTable(
                name: "InventoryForms",
                newName: "InventoryForm");

            migrationBuilder.RenameTable(
                name: "CheckItems",
                newName: "CheckItem");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardTyreBatteries_JobCardId",
                table: "JobCardTyreBattery",
                newName: "IX_JobCardTyreBattery_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardEstimationItems_JobCardId",
                table: "JobCardEstimationItem",
                newName: "IX_JobCardEstimationItem_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardConcerns_JobCardId",
                table: "JobCardConcern",
                newName: "IX_JobCardConcern_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardCollections_JobCardId",
                table: "JobCardCollection",
                newName: "IX_JobCardCollection_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardCancelledInvoices_JobCardId",
                table: "JobCardCancelledInvoice",
                newName: "IX_JobCardCancelledInvoice_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardAdvancePayments_JobCardId",
                table: "JobCardAdvancePayment",
                newName: "IX_JobCardAdvancePayment_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckItems_TechnicianMCId",
                table: "CheckItem",
                newName: "IX_CheckItem_TechnicianMCId");

            // 1. Drop Primary Key
            migrationBuilder.DropPrimaryKey(
                name: "PK_ToBeFilledBySupervisor",
                table: "ToBeFilledBySupervisor");

            // 2. Drop Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ToBeFilledBySupervisor");

            // 3. Recreate Id column as BIGINT IDENTITY
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // 4. Recreate Primary Key
            migrationBuilder.AddPrimaryKey(
                name: "PK_ToBeFilledBySupervisor",
                table: "ToBeFilledBySupervisor",
                column: "Id");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "User",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "User",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "TechnicianMC",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RepairOrder",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "LabourDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "InventoryForm",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "TechnicianMCId",
                table: "CheckItem",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CheckItem",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnicianMC",
                table: "TechnicianMC",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SparePartsIssueDetail",
                table: "SparePartsIssueDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairOrder",
                table: "RepairOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabourDetail",
                table: "LabourDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardTyreBattery",
                table: "JobCardTyreBattery",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCard",
                table: "JobCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardEstimationItem",
                table: "JobCardEstimationItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardConcern",
                table: "JobCardConcern",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardCollection",
                table: "JobCardCollection",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardCancelledInvoice",
                table: "JobCardCancelledInvoice",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardAdvancePayment",
                table: "JobCardAdvancePayment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryForm",
                table: "InventoryForm",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckItem",
                table: "CheckItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAccessory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false),
                    InventoryFormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAccessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAccessory_InventoryForm_InventoryFormId",
                        column: x => x.InventoryFormId,
                        principalTable: "InventoryForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    RegPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId1 = table.Column<long>(type: "bigint", nullable: true),
                    WorkshopProfileId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkshopUser_User_UserId1",
                        column: x => x.UserId1,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkshopUser_WorkshopProfile_WorkshopProfileId",
                        column: x => x.WorkshopProfileId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookAppointment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceAdvisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    UserId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAppointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookAppointment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAppointment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookAppointment_User_UserId1",
                        column: x => x.UserId1,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookAppointment_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookAppointment_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_CustomerId",
                table: "BookAppointment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_UserId",
                table: "BookAppointment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_UserId1",
                table: "BookAppointment",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_VehicleId",
                table: "BookAppointment",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_WorkshopId",
                table: "BookAppointment",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccessory_InventoryFormId",
                table: "InventoryAccessory",
                column: "InventoryFormId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CustomerId",
                table: "Vehicle",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_UserId",
                table: "WorkshopUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_UserId1",
                table: "WorkshopUser",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_WorkshopId",
                table: "WorkshopUser",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_WorkshopProfileId",
                table: "WorkshopUser",
                column: "WorkshopProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckItem_TechnicianMC_TechnicianMCId",
                table: "CheckItem",
                column: "TechnicianMCId",
                principalTable: "TechnicianMC",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardAdvancePayment_JobCard_JobCardId",
                table: "JobCardAdvancePayment",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardCancelledInvoice_JobCard_JobCardId",
                table: "JobCardCancelledInvoice",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardCollection_JobCard_JobCardId",
                table: "JobCardCollection",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardConcern_JobCard_JobCardId",
                table: "JobCardConcern",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardEstimationItem_JobCard_JobCardId",
                table: "JobCardEstimationItem",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardTyreBattery_JobCard_JobCardId",
                table: "JobCardTyreBattery",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.Sql(@"
    IF NOT EXISTS (SELECT 1 FROM [Role])
    BEGIN
        INSERT INTO [Role] (RoleName)
        VALUES ('Admin');
    END
");
            migrationBuilder.Sql(@"
    DECLARE @DefaultRoleId BIGINT;

    SELECT @DefaultRoleId = Id
    FROM [Role]
    WHERE RoleName = 'Admin';

    UPDATE u
    SET RoleId = @DefaultRoleId
    FROM [User] u
    LEFT JOIN [Role] r ON r.Id = u.RoleId
    WHERE r.Id IS NULL;
");




            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckItem_TechnicianMC_TechnicianMCId",
                table: "CheckItem");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardAdvancePayment_JobCard_JobCardId",
                table: "JobCardAdvancePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardCancelledInvoice_JobCard_JobCardId",
                table: "JobCardCancelledInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardCollection_JobCard_JobCardId",
                table: "JobCardCollection");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardConcern_JobCard_JobCardId",
                table: "JobCardConcern");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardEstimationItem_JobCard_JobCardId",
                table: "JobCardEstimationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardTyreBattery_JobCard_JobCardId",
                table: "JobCardTyreBattery");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DropTable(
                name: "BookAppointment");

            migrationBuilder.DropTable(
                name: "InventoryAccessory");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "WorkshopUser");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "WorkshopProfile");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnicianMC",
                table: "TechnicianMC");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SparePartsIssueDetail",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairOrder",
                table: "RepairOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabourDetail",
                table: "LabourDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardTyreBattery",
                table: "JobCardTyreBattery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardEstimationItem",
                table: "JobCardEstimationItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardConcern",
                table: "JobCardConcern");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardCollection",
                table: "JobCardCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardCancelledInvoice",
                table: "JobCardCancelledInvoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCardAdvancePayment",
                table: "JobCardAdvancePayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCard",
                table: "JobCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryForm",
                table: "InventoryForm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckItem",
                table: "CheckItem");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TechnicianMC",
                newName: "TechnicianMCForms");

            migrationBuilder.RenameTable(
                name: "SparePartsIssueDetail",
                newName: "SparePartsIssueDetails");

            migrationBuilder.RenameTable(
                name: "RepairOrder",
                newName: "RepairOrders");

            migrationBuilder.RenameTable(
                name: "LabourDetail",
                newName: "LabourDetails");

            migrationBuilder.RenameTable(
                name: "JobCardTyreBattery",
                newName: "JobCardTyreBatteries");

            migrationBuilder.RenameTable(
                name: "JobCardEstimationItem",
                newName: "JobCardEstimationItems");

            migrationBuilder.RenameTable(
                name: "JobCardConcern",
                newName: "JobCardConcerns");

            migrationBuilder.RenameTable(
                name: "JobCardCollection",
                newName: "JobCardCollections");

            migrationBuilder.RenameTable(
                name: "JobCardCancelledInvoice",
                newName: "JobCardCancelledInvoices");

            migrationBuilder.RenameTable(
                name: "JobCardAdvancePayment",
                newName: "JobCardAdvancePayments");

            migrationBuilder.RenameTable(
                name: "JobCard",
                newName: "JobCards");

            migrationBuilder.RenameTable(
                name: "InventoryForm",
                newName: "InventoryForms");

            migrationBuilder.RenameTable(
                name: "CheckItem",
                newName: "CheckItems");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardTyreBattery_JobCardId",
                table: "JobCardTyreBatteries",
                newName: "IX_JobCardTyreBatteries_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardEstimationItem_JobCardId",
                table: "JobCardEstimationItems",
                newName: "IX_JobCardEstimationItems_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardConcern_JobCardId",
                table: "JobCardConcerns",
                newName: "IX_JobCardConcerns_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardCollection_JobCardId",
                table: "JobCardCollections",
                newName: "IX_JobCardCollections_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardCancelledInvoice_JobCardId",
                table: "JobCardCancelledInvoices",
                newName: "IX_JobCardCancelledInvoices_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCardAdvancePayment_JobCardId",
                table: "JobCardAdvancePayments",
                newName: "IX_JobCardAdvancePayments_JobCardId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckItem_TechnicianMCId",
                table: "CheckItems",
                newName: "IX_CheckItems_TechnicianMCId");

            migrationBuilder.DropPrimaryKey(
         name: "PK_ToBeFilledBySupervisor",
         table: "ToBeFilledBySupervisor");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ToBeFilledBySupervisor",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToBeFilledBySupervisor",
                table: "ToBeFilledBySupervisor",
                column: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TechnicianMCForms",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SparePartsIssueDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RepairOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LabourDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "InventoryForms",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianMCId",
                table: "CheckItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CheckItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnicianMCForms",
                table: "TechnicianMCForms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SparePartsIssueDetails",
                table: "SparePartsIssueDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairOrders",
                table: "RepairOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabourDetails",
                table: "LabourDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardTyreBatteries",
                table: "JobCardTyreBatteries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardEstimationItems",
                table: "JobCardEstimationItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardConcerns",
                table: "JobCardConcerns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardCollections",
                table: "JobCardCollections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardCancelledInvoices",
                table: "JobCardCancelledInvoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCardAdvancePayments",
                table: "JobCardAdvancePayments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCards",
                table: "JobCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryForms",
                table: "InventoryForms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckItems",
                table: "CheckItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BookAppointments",
                columns: table => new
                {
                    search = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    bay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    emailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    regNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    regPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serviceAdvisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    settings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAppointments", x => x.search);
                    table.ForeignKey(
                        name: "FK_BookAppointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryAccessories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryFormId = table.Column<int>(type: "int", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAccessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAccessories_InventoryForms_InventoryFormId",
                        column: x => x.InventoryFormId,
                        principalTable: "InventoryForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopProfiles",
                columns: table => new
                {
                    WorkshopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopProfiles", x => x.WorkshopId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointments_UserId",
                table: "BookAppointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccessories_InventoryFormId",
                table: "InventoryAccessories",
                column: "InventoryFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckItems_TechnicianMCForms_TechnicianMCId",
                table: "CheckItems",
                column: "TechnicianMCId",
                principalTable: "TechnicianMCForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardAdvancePayments_JobCards_JobCardId",
                table: "JobCardAdvancePayments",
                column: "JobCardId",
                principalTable: "JobCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardCancelledInvoices_JobCards_JobCardId",
                table: "JobCardCancelledInvoices",
                column: "JobCardId",
                principalTable: "JobCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardCollections_JobCards_JobCardId",
                table: "JobCardCollections",
                column: "JobCardId",
                principalTable: "JobCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardConcerns_JobCards_JobCardId",
                table: "JobCardConcerns",
                column: "JobCardId",
                principalTable: "JobCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardEstimationItems_JobCards_JobCardId",
                table: "JobCardEstimationItems",
                column: "JobCardId",
                principalTable: "JobCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardTyreBatteries_JobCards_JobCardId",
                table: "JobCardTyreBatteries",
                column: "JobCardId",
                principalTable: "JobCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
