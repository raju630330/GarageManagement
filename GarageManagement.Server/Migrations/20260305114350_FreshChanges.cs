using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class FreshChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail");

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
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_PermissionModule_PermissionModuleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_Part_PartId",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianMC_RepairOrder_RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropForeignKey(
                name: "FK_ToBeFilledBySupervisor_RepairOrder_RepairOrderId",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Customer_CustomerId",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckItem_TechnicianMC_TechnicianMCId",
                table: "CheckItem",
                column: "TechnicianMCId",
                principalTable: "TechnicianMC",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardAdvancePayment_JobCard_JobCardId",
                table: "JobCardAdvancePayment",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardCancelledInvoice_JobCard_JobCardId",
                table: "JobCardCancelledInvoice",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardCollection_JobCard_JobCardId",
                table: "JobCardCollection",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardConcern_JobCard_JobCardId",
                table: "JobCardConcern",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardEstimationItem_JobCard_JobCardId",
                table: "JobCardEstimationItem",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardTyreBattery_JobCard_JobCardId",
                table: "JobCardTyreBattery",
                column: "JobCardId",
                principalTable: "JobCard",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_PermissionModule_PermissionModuleId",
                table: "RolePermission",
                column: "PermissionModuleId",
                principalTable: "PermissionModule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                table: "SparePartsIssueDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_Part_PartId",
                table: "StockMovement",
                column: "PartId",
                principalTable: "Part",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Customer_CustomerId",
                table: "Vehicle",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail");

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
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_PermissionModule_PermissionModuleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                table: "SparePartsIssueDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_Part_PartId",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianMC_RepairOrder_RepairOrderId",
                table: "TechnicianMC");

            migrationBuilder.DropForeignKey(
                name: "FK_ToBeFilledBySupervisor_RepairOrder_RepairOrderId",
                table: "ToBeFilledBySupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Customer_CustomerId",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                table: "AdditionalJobObserveDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                table: "LabourDetail",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_PermissionModule_PermissionModuleId",
                table: "RolePermission",
                column: "PermissionModuleId",
                principalTable: "PermissionModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Role",
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
                name: "FK_StockMovement_Part_PartId",
                table: "StockMovement",
                column: "PartId",
                principalTable: "Part",
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

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Customer_CustomerId",
                table: "Vehicle",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                table: "WorkshopUser",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
