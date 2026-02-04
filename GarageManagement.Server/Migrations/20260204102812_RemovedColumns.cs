using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "PartNo",
                table: "JobCardEstimationItem");

            migrationBuilder.AddColumn<long>(
                name: "PartId",
                table: "JobCardEstimationItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCardEstimationItem_PartId",
                table: "JobCardEstimationItem",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCardEstimationItem_Part_PartId",
                table: "JobCardEstimationItem",
                column: "PartId",
                principalTable: "Part",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCardEstimationItem_Part_PartId",
                table: "JobCardEstimationItem");

            migrationBuilder.DropIndex(
                name: "IX_JobCardEstimationItem_PartId",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "PartId",
                table: "JobCardEstimationItem");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "JobCardEstimationItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartNo",
                table: "JobCardEstimationItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
