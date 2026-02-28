using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class IncludedWorkshopIdinallTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopProfile_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_User_UserId",
                table: "WorkshopUser");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_User_UserId1",
                table: "WorkshopUser");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser");

            migrationBuilder.DropIndex(
                name: "IX_WorkshopUser_UserId1",
                table: "WorkshopUser");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "WorkshopUser");

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "WorkshopProfile",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "Vehicle",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "TechnicianMC",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "StockMovement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "Service",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "Role",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "RepairOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "PermissionModule",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "Permission",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "PaymentMode",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "Part",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "LabourDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "JobCardTyreBattery",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "JobCardEstimationItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "JobCardConcern",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "JobCardCollection",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "JobCardCancelledInvoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "JobCardAdvancePayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "JobCard",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "InventoryForm",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "InventoryAccessory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "Customer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "CheckItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkshopId",
                table: "AdditionalJobObserveDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopProfile_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile",
                column: "ParentWorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_User_UserId",
                table: "WorkshopUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopProfile_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_User_UserId",
                table: "WorkshopUser");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "TechnicianMC");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "PermissionModule");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "PaymentMode");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "LabourDetail");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "JobCardTyreBattery");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "JobCardConcern");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "JobCardCollection");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "JobCardCancelledInvoice");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "JobCardAdvancePayment");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "InventoryForm");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "InventoryAccessory");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "CheckItem");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "WorkshopUser",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_UserId1",
                table: "WorkshopUser",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopProfile_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile",
                column: "ParentWorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_User_UserId",
                table: "WorkshopUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_User_UserId1",
                table: "WorkshopUser",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
