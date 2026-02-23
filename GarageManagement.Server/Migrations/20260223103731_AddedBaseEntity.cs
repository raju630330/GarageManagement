using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopWorkingDay",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopWorkingDay",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopWorkingDay",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopUser",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopUser",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>( 
                name: "ModifiedBy",
                table: "WorkshopTiming",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopTiming",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopTiming",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "WorkshopService",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopService",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopService",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopProfile",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopProfile",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "WorkshopPaymentMode",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopPaymentMode",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopPaymentMode",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopPaymentMode",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopMedia",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopMedia",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopMedia",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopBusinessConfig",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopBusinessConfig",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopBusinessConfig",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopAddress",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopAddress",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "WorkshopAddress",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Vehicle",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Vehicle",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "Vehicle",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "User",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ToBeFilledBySupervisor",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "ToBeFilledBySupervisor",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TechnicianSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FloorSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "TechnicianMC",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "TechnicianMC",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "TechnicianMC",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "StockMovement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "StockMovement",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "StockMovement",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                table: "SparePartsIssueDetail",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "SparePartsIssueDetail",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "SparePartsIssueDetail",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SparePartsIssueDetail",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "SparePartsIssueDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "SparePartsIssueDetail",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Service",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Service",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "Service",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Role",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "Role",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VinNumber",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "RepairOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "RepairOrder",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "RepairOrder",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "PermissionModule",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "PermissionModule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "PermissionModule",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Permission",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "Permission",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "PaymentMode",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "PaymentMode",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "PaymentMode",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Part",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Part",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "Part",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "LabourDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "LabourDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "LabourDetail",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "JobCardTyreBattery",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardTyreBattery",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "JobCardTyreBattery",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "JobCardEstimationItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardEstimationItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "JobCardEstimationItem",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "JobCardConcern",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardConcern",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "JobCardConcern",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "JobCardCollection",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardCollection",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "JobCardCollection",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "JobCardCancelledInvoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardCancelledInvoice",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "JobCardCancelledInvoice",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "JobCardAdvancePayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardAdvancePayment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "JobCardAdvancePayment",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "JobCard",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCard",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "JobCard",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "InventoryForm",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "InventoryForm",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "InventoryForm",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "InventoryAccessory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "InventoryAccessory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "InventoryAccessory",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Customer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "Customer",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Control",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "CheckItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "CheckItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "CheckItem",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "BookAppointment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "BookAppointment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "BookAppointment",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "AdditionalJobObserveDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "AdditionalJobObserveDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RowState",
                table: "AdditionalJobObserveDetail",
                type: "tinyint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopWorkingDay");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopWorkingDay");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopWorkingDay");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopUser");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopUser");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopUser");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopTiming");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopTiming");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopTiming");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkshopService");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopService");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopService");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopService");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkshopPaymentMode");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopPaymentMode");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopPaymentMode");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopPaymentMode");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopMedia");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopMedia");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopMedia");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopBusinessConfig");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopBusinessConfig");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopBusinessConfig");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WorkshopAddress");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WorkshopAddress");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "WorkshopAddress");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "TechnicianMC");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "TechnicianMC");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "TechnicianMC");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PermissionModule");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PermissionModule");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "PermissionModule");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PaymentMode");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PaymentMode");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "PaymentMode");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "LabourDetail");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "LabourDetail");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "LabourDetail");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "JobCardTyreBattery");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "JobCardTyreBattery");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "JobCardTyreBattery");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "JobCardConcern");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "JobCardConcern");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "JobCardConcern");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "JobCardCollection");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "JobCardCollection");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "JobCardCollection");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "JobCardCancelledInvoice");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "JobCardCancelledInvoice");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "JobCardCancelledInvoice");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "JobCardAdvancePayment");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "JobCardAdvancePayment");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "JobCardAdvancePayment");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "InventoryForm");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "InventoryForm");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "InventoryForm");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "InventoryAccessory");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "InventoryAccessory");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "InventoryAccessory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CheckItem");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "CheckItem");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "CheckItem");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "BookAppointment");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "BookAppointment");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "BookAppointment");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.DropColumn(
                name: "RowState",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.AlterColumn<string>(
                name: "TechnicianSign",
                table: "TechnicianMC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "TechnicianMC",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FloorSign",
                table: "TechnicianMC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverSign",
                table: "TechnicianMC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthSign",
                table: "TechnicianMC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                table: "SparePartsIssueDetail",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "SparePartsIssueDetail",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "SparePartsIssueDetail",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SparePartsIssueDetail",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VinNumber",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CheckItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "CheckItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Control",
                table: "CheckItem",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
