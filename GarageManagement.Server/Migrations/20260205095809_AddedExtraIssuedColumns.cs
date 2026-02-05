using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraIssuedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "JobCardEstimationItem",
                newName: "RequestedQuantity");

            migrationBuilder.AddColumn<string>(
                name: "IssuedBy",
                table: "JobCardEstimationItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuedDate",
                table: "JobCardEstimationItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IssuedId",
                table: "JobCardEstimationItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedBy",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "IssuedDate",
                table: "JobCardEstimationItem");

            migrationBuilder.DropColumn(
                name: "IssuedId",
                table: "JobCardEstimationItem");

            migrationBuilder.RenameColumn(
                name: "RequestedQuantity",
                table: "JobCardEstimationItem",
                newName: "Quantity");
        }
    }
}
