using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsInStockMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EstimationItemId",
                table: "StockMovement",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_EstimationItemId",
                table: "StockMovement",
                column: "EstimationItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_JobCardEstimationItem_EstimationItemId",
                table: "StockMovement",
                column: "EstimationItemId",
                principalTable: "JobCardEstimationItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_JobCardEstimationItem_EstimationItemId",
                table: "StockMovement");

            migrationBuilder.DropIndex(
                name: "IX_StockMovement_EstimationItemId",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "EstimationItemId",
                table: "StockMovement");
        }
    }
}
