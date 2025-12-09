using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class MyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalJobObserveDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicianVoice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorInstructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalJobObserveDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OdometerIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvgKmsPerDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngineNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceAdvisor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Technician = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Corporate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternateMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCardAdvancePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    Cash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardAdvancePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardAdvancePayments_JobCards_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCardConcerns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardConcerns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardConcerns_JobCards_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobCardAdvancePayments_JobCardId",
                table: "JobCardAdvancePayments",
                column: "JobCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCardConcerns_JobCardId",
                table: "JobCardConcerns",
                column: "JobCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalJobObserveDetail");

            migrationBuilder.DropTable(
                name: "JobCardAdvancePayments");

            migrationBuilder.DropTable(
                name: "JobCardConcerns");

            migrationBuilder.DropTable(
                name: "JobCards");
        }
    }
}
