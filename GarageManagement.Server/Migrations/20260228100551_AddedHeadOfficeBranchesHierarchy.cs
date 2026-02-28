using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedHeadOfficeBranchesHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkshopProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ParentWorkshopId",
                table: "WorkshopProfile",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile",
                column: "ParentWorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopProfile_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile",
                column: "ParentWorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopProfile_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile");

            migrationBuilder.DropIndex(
                name: "IX_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "ParentWorkshopId",
                table: "WorkshopProfile");
        }
    }
}
