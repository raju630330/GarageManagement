using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "ToBeFilledBySupervisor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "TechnicianMC",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "SparePartsIssueDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BookingAppointmentId",
                table: "RepairOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "LabourDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "InventoryAccessory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "AdditionalJobObserveDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToBeFilledBySupervisor_RepairOrderId",
                table: "ToBeFilledBySupervisor",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianMC_RepairOrderId",
                table: "TechnicianMC",
                column: "RepairOrderId",
                unique: true,
                filter: "[RepairOrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartsIssueDetail_RepairOrderId",
                table: "SparePartsIssueDetail",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrder_BookingAppointmentId",
                table: "RepairOrder",
                column: "BookingAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LabourDetail_RepairOrderId",
                table: "LabourDetail",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccessory_RepairOrderId",
                table: "InventoryAccessory",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalJobObserveDetail_RepairOrderId",
                table: "AdditionalJobObserveDetail",
                column: "RepairOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAccessory_RepairOrder_RepairOrderId",
                table: "InventoryAccessory",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrder_BookAppointment_BookingAppointmentId",
                table: "RepairOrder",
                column: "BookingAppointmentId",
                principalTable: "BookAppointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAccessory_RepairOrder_RepairOrderId",
                table: "InventoryAccessory");

            migrationBuilder.DropForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrder_BookAppointment_BookingAppointmentId",
                table: "RepairOrder");

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
                name: "IX_ToBeFilledBySupervisor_RepairOrderId",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianMC_RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropIndex(
                name: "IX_SparePartsIssueDetail_RepairOrderId",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrder_BookingAppointmentId",
                table: "RepairOrder");

            migrationBuilder.DropIndex(
                name: "IX_LabourDetail_RepairOrderId",
                table: "LabourDetail");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAccessory_RepairOrderId",
                table: "InventoryAccessory");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalJobObserveDetail_RepairOrderId",
                table: "AdditionalJobObserveDetail");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropColumn(
                name: "BookingAppointmentId",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "LabourDetail");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "InventoryAccessory");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "AdditionalJobObserveDetail");
        }
    }
}
