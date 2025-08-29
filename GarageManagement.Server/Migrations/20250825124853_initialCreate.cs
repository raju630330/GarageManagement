using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                    regNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vehicle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serviceAdvisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    settings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkshopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookAppointments", x => x.search);
                    table.ForeignKey(
                        name: "FK_bookAppointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookAppointments_WorkshopProfiles_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfiles",
                        principalColumn: "WorkshopId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookAppointments_UserId",
                table: "bookAppointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_bookAppointments_WorkshopId",
                table: "bookAppointments",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                table: "Users",
                column: "Role",
                unique: true,
                filter: "[Role] = 'Admin'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookAppointments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkshopProfiles");
        }
    }
}
