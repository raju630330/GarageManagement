using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabourDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabourCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutsideLabour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabourDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VinNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mkls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteInchargeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderWarranty = table.Column<bool>(type: "bit", nullable: false),
                    ExpectedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AllottedTechnician = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpiresUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopProfiles",
                columns: table => new
                {
                    WorkshopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopProfiles", x => x.WorkshopId);
                });

            migrationBuilder.CreateTable(
                name: "bookAppointments",
                columns: table => new
                {
                    search = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    regPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    regNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serviceAdvisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    settings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookAppointments", x => x.search);
                    table.ForeignKey(
                        name: "FK_bookAppointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookAppointments_UserId",
                table: "bookAppointments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookAppointments");

            migrationBuilder.DropTable(
                name: "LabourDetails");

            migrationBuilder.DropTable(
                name: "RepairOrders");

            migrationBuilder.DropTable(
                name: "WorkshopProfiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
