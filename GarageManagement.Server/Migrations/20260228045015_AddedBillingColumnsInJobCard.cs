using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedBillingColumnsInJobCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "JobCard",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "JobCard",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "JobCard",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RoundOffAmount",
                table: "JobCard",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "RoundOffAmount",
                table: "JobCard");
        }
    }
}
