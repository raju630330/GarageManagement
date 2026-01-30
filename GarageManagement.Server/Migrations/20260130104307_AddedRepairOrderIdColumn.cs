using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedRepairOrderIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RepairOrderId",
                table: "JobCard",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCard_RepairOrderId",
                table: "JobCard",
                column: "RepairOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCard_RepairOrder_RepairOrderId",
                table: "JobCard",
                column: "RepairOrderId",
                principalTable: "RepairOrder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCard_RepairOrder_RepairOrderId",
                table: "JobCard");

            migrationBuilder.DropIndex(
                name: "IX_JobCard_RepairOrderId",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "JobCard");
        }
    }
}
