using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class NullValuesConvertedIntoMandatory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAppointment_Customer_CustomerId",
                table: "BookAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAppointment_WorkshopProfile_WorkshopId",
                table: "BookAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryForm_RepairOrder_RepairOrderId",
                table: "InventoryForm");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCard_RepairOrder_RepairOrderId",
                table: "JobCard");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardEstimationItem_Part_PartId",
                table: "JobCardEstimationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianMC_RepairOrder_RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropForeignKey(
                name: "FK_ToBeFilledBySupervisor_RepairOrder_RepairOrderId",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianMC_RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropIndex(
                name: "IX_InventoryForm_RepairOrderId",
                table: "InventoryForm");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopWorkingDay",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopWorkingDay",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopWorkingDay",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopUser",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopUser",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopTiming",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopTiming",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopTiming",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopService",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopService",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopService",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopProfile",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopProfile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopProfile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopPaymentMode",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopPaymentMode",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopPaymentMode",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopMedia",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopMedia",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopMedia",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopBusinessConfig",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopBusinessConfig",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopBusinessConfig",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopAddress",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopAddress",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopAddress",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Vehicle",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Vehicle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Vehicle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "User",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "User",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "ToBeFilledBySupervisor",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "ToBeFilledBySupervisor",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TechnicianSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "TechnicianMC",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "TechnicianMC",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "TechnicianMC",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "TechnicianMC",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FloorSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "StockMovement",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "StockMovement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "StockMovement",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "SparePartsIssueDetail",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "SparePartsIssueDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Service",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Service",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Service",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PermissionModuleId",
                table: "RolePermission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PermissionId",
                table: "RolePermission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Role",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Role",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Role",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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
                name: "VehicleSite",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfService",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SiteInchargeName",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "RepairOrder",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "RoadTestAlongWithDriver",
                table: "RepairOrder",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RepairEstimationCost",
                table: "RepairOrder",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "RepairOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "RepairOrder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mkls",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpectedDateTime",
                table: "RepairOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "DriverPermanetToThisVehicle",
                table: "RepairOrder",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverName",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BookingAppointmentId",
                table: "RepairOrder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AllottedTechnician",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "PermissionModule",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "PermissionModule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "PermissionModule",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Permission",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Permission",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Permission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "PaymentMode",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "PaymentMode",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "PaymentMode",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Part",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Part",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Part",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "LabourDetail",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "LabourDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "LabourDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "LabourDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LabourDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardTyreBattery",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardTyreBattery",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardTyreBattery",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardEstimationItem",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PartId",
                table: "JobCardEstimationItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardEstimationItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardEstimationItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "JobCardConcern",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardConcern",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardConcern",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardConcern",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardCollection",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardCollection",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardCollection",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardCancelledInvoice",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardCancelledInvoice",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardCancelledInvoice",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardAdvancePayment",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardAdvancePayment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardAdvancePayment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "JobCardAdvancePayment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChequeNo",
                table: "JobCardAdvancePayment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cash",
                table: "JobCardAdvancePayment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "JobCardAdvancePayment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "JobCardAdvancePayment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "JobCard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCard",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "JobCard",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OdometerIn",
                table: "JobCard",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCard",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCard",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobCardNo",
                table: "JobCard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "JobCard",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AvgKmsPerDay",
                table: "JobCard",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "InventoryForm",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "InventoryForm",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "InventoryForm",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "InventoryForm",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "InventoryAccessory",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "InventoryAccessory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "InventoryAccessory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Customer",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Customer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "CheckItem",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "CheckItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "CheckItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Control",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "WorkshopId",
                table: "BookAppointment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VehicleId",
                table: "BookAppointment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "BookAppointment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "BookAppointment",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "BookAppointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "BookAppointment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VehicleId1",
                table: "BookAppointment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "AdditionalJobObserveDetail",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "AdditionalJobObserveDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "AdditionalJobObserveDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "AdditionalJobObserveDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianMC_RepairOrderId",
                table: "TechnicianMC",
                column: "RepairOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryForm_RepairOrderId",
                table: "InventoryForm",
                column: "RepairOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_VehicleId1",
                table: "BookAppointment",
                column: "VehicleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAppointment_Customer_CustomerId",
                table: "BookAppointment",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAppointment_Vehicle_VehicleId1",
                table: "BookAppointment",
                column: "VehicleId1",
                principalTable: "Vehicle",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAppointment_WorkshopProfile_WorkshopId",
                table: "BookAppointment",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryForm_RepairOrder_RepairOrderId",
                table: "InventoryForm",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCard_RepairOrder_RepairOrderId",
                table: "JobCard",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardEstimationItem_Part_PartId",
                table: "JobCardEstimationItem",
                column: "PartId",
                principalTable: "Part",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                table: "SparePartsIssueDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianMC_RepairOrder_RepairOrderId",
                table: "TechnicianMC",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToBeFilledBySupervisor_RepairOrder_RepairOrderId",
                table: "ToBeFilledBySupervisor",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAppointment_Customer_CustomerId",
                table: "BookAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAppointment_Vehicle_VehicleId1",
                table: "BookAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAppointment_WorkshopProfile_WorkshopId",
                table: "BookAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryForm_RepairOrder_RepairOrderId",
                table: "InventoryForm");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCard_RepairOrder_RepairOrderId",
                table: "JobCard");

            migrationBuilder.DropForeignKey(
                name: "FK_JobCardEstimationItem_Part_PartId",
                table: "JobCardEstimationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianMC_RepairOrder_RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropForeignKey(
                name: "FK_ToBeFilledBySupervisor_RepairOrder_RepairOrderId",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianMC_RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropIndex(
                name: "IX_InventoryForm_RepairOrderId",
                table: "InventoryForm");

            migrationBuilder.DropIndex(
                name: "IX_BookAppointment_VehicleId1",
                table: "BookAppointment");

            migrationBuilder.DropColumn(
                name: "VehicleId1",
                table: "BookAppointment");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopWorkingDay",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopWorkingDay",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopWorkingDay",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopUser",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopUser",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopUser",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopTiming",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopTiming",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopTiming",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopService",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopService",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopService",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopProfile",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopProfile",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopProfile",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopPaymentMode",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopPaymentMode",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopPaymentMode",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopMedia",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopMedia",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopMedia",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopBusinessConfig",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopBusinessConfig",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopBusinessConfig",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "WorkshopAddress",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "WorkshopAddress",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "WorkshopAddress",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Vehicle",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Vehicle",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Vehicle",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "User",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "User",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "ToBeFilledBySupervisor",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "ToBeFilledBySupervisor",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "TechnicianSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "TechnicianMC",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "TechnicianMC",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "TechnicianMC",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "TechnicianMC",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "FloorSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DriverSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthSign",
                table: "TechnicianMC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "StockMovement",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "StockMovement",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "StockMovement",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "SparePartsIssueDetail",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "SparePartsIssueDetail",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Service",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Service",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Service",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "PermissionModuleId",
                table: "RolePermission",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "PermissionId",
                table: "RolePermission",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Role",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Role",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Role",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "VinNumber",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleSite",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfService",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SiteInchargeName",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "RepairOrder",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<bool>(
                name: "RoadTestAlongWithDriver",
                table: "RepairOrder",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "RepairEstimationCost",
                table: "RepairOrder",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "RepairOrder",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "RepairOrder",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Mkls",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpectedDateTime",
                table: "RepairOrder",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "DriverPermanetToThisVehicle",
                table: "RepairOrder",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DriverName",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "BookingAppointmentId",
                table: "RepairOrder",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "AllottedTechnician",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "PermissionModule",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "PermissionModule",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "PermissionModule",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Permission",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Permission",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Permission",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "PaymentMode",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "PaymentMode",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "PaymentMode",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Part",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Part",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Part",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "LabourDetail",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "LabourDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "LabourDetail",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "LabourDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LabourDetail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardTyreBattery",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardTyreBattery",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardTyreBattery",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardEstimationItem",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "PartId",
                table: "JobCardEstimationItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardEstimationItem",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardEstimationItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "JobCardConcern",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardConcern",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardConcern",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardConcern",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardCollection",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardCollection",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardCollection",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardCancelledInvoice",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardCancelledInvoice",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardCancelledInvoice",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCardAdvancePayment",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCardAdvancePayment",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCardAdvancePayment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "JobCardAdvancePayment",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ChequeNo",
                table: "JobCardAdvancePayment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cash",
                table: "JobCardAdvancePayment",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "JobCardAdvancePayment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "JobCardAdvancePayment",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "JobCard",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "JobCard",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "JobCard",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "OdometerIn",
                table: "JobCard",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "JobCard",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "JobCard",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "JobCardNo",
                table: "JobCard",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "JobCard",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "AvgKmsPerDay",
                table: "JobCard",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "InventoryForm",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "InventoryForm",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "InventoryForm",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "InventoryForm",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "InventoryAccessory",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "InventoryAccessory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "InventoryAccessory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "Customer",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "Customer",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "Customer",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "CheckItem",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "CheckItem",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "CheckItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Control",
                table: "CheckItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "WorkshopId",
                table: "BookAppointment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "VehicleId",
                table: "BookAppointment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "BookAppointment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "BookAppointment",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "BookAppointment",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "BookAppointment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<byte>(
                name: "RowState",
                table: "AdditionalJobObserveDetail",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<long>(
                name: "RepairOrderId",
                table: "AdditionalJobObserveDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                table: "AdditionalJobObserveDetail",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "ModifiedBy",
                table: "AdditionalJobObserveDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianMC_RepairOrderId",
                table: "TechnicianMC",
                column: "RepairOrderId",
                unique: true,
                filter: "[RepairOrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryForm_RepairOrderId",
                table: "InventoryForm",
                column: "RepairOrderId",
                unique: true,
                filter: "[RepairOrderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAppointment_Customer_CustomerId",
                table: "BookAppointment",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAppointment_WorkshopProfile_WorkshopId",
                table: "BookAppointment",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryForm_RepairOrder_RepairOrderId",
                table: "InventoryForm",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCard_RepairOrder_RepairOrderId",
                table: "JobCard",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardEstimationItem_Part_PartId",
                table: "JobCardEstimationItem",
                column: "PartId",
                principalTable: "Part",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                table: "SparePartsIssueDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianMC_RepairOrder_RepairOrderId",
                table: "TechnicianMC",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToBeFilledBySupervisor_RepairOrder_RepairOrderId",
                table: "ToBeFilledBySupervisor",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");
        }
    }
}
