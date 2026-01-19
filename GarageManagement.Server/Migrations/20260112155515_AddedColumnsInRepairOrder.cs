using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsInRepairOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DriverPermanetToThisVehicle",
                table: "RepairOrder",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RepairEstimationCost",
                table: "RepairOrder",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RoadTestAlongWithDriver",
                table: "RepairOrder",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfService",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "DriverPermanetToThisVehicle",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "RepairEstimationCost",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "RoadTestAlongWithDriver",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "TypeOfService",
                table: "RepairOrder");
        }
    }
}
