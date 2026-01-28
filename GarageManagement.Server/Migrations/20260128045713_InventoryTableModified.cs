using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class InventoryTableModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAccessory_RepairOrder_RepairOrderId",
                table: "InventoryAccessory");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAccessory_RepairOrderId",
                table: "InventoryAccessory");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "InventoryAccessory");

            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "InventoryForm",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryForm_RepairOrderId",
                table: "InventoryForm",
                column: "RepairOrderId",
                unique: true,
                filter: "[RepairOrderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryForm_RepairOrder_RepairOrderId",
                table: "InventoryForm",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryForm_RepairOrder_RepairOrderId",
                table: "InventoryForm");

            migrationBuilder.DropIndex(
                name: "IX_InventoryForm_RepairOrderId",
                table: "InventoryForm");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "InventoryForm");

            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "InventoryAccessory",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccessory_RepairOrderId",
                table: "InventoryAccessory",
                column: "RepairOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAccessory_RepairOrder_RepairOrderId",
                table: "InventoryAccessory",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");
        }
    }
}
