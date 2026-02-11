using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class StockMovementModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "StockMovement",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_UserId",
                table: "StockMovement",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_User_UserId",
                table: "StockMovement",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_User_UserId",
                table: "StockMovement");

            migrationBuilder.DropIndex(
                name: "IX_StockMovement_UserId",
                table: "StockMovement");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StockMovement");
        }
    }
}
