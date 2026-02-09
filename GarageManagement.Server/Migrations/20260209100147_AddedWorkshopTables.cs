using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedWorkshopTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopProfileId",
                table: "WorkshopUser");

            migrationBuilder.DropIndex(
                name: "IX_WorkshopUser_WorkshopProfileId",
                table: "WorkshopUser");

            migrationBuilder.DropColumn(
                name: "WorkshopProfileId",
                table: "WorkshopUser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalendarDate",
                table: "WorkshopProfile",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "AvgVehicleInflowPerMonth",
                table: "WorkshopProfile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DealerCode",
                table: "WorkshopProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InBusinessSince",
                table: "WorkshopProfile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsGdprAccepted",
                table: "WorkshopProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NoOfEmployees",
                table: "WorkshopProfile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentMode",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopAddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    FlatNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landmark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopAddress_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopBusinessConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    WebsiteLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleReviewLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalIntegrationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSTIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MSME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SACPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvoiceCaption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopBusinessConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopBusinessConfig_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopMedia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopMedia_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopTiming",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopTiming", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopTiming_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopWorkingDay",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopWorkingDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopWorkingDay_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopPaymentMode",
                columns: table => new
                {
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopPaymentMode", x => new { x.WorkshopId, x.PaymentModeId });
                    table.ForeignKey(
                        name: "FK_WorkshopPaymentMode_PaymentMode_PaymentModeId",
                        column: x => x.PaymentModeId,
                        principalTable: "PaymentMode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkshopPaymentMode_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopService",
                columns: table => new
                {
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopService", x => new { x.WorkshopId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_WorkshopService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkshopService_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopAddress_WorkshopId",
                table: "WorkshopAddress",
                column: "WorkshopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopBusinessConfig_WorkshopId",
                table: "WorkshopBusinessConfig",
                column: "WorkshopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopMedia_WorkshopId",
                table: "WorkshopMedia",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopPaymentMode_PaymentModeId",
                table: "WorkshopPaymentMode",
                column: "PaymentModeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopService_ServiceId",
                table: "WorkshopService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopTiming_WorkshopId",
                table: "WorkshopTiming",
                column: "WorkshopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopWorkingDay_WorkshopId",
                table: "WorkshopWorkingDay",
                column: "WorkshopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkshopAddress");

            migrationBuilder.DropTable(
                name: "WorkshopBusinessConfig");

            migrationBuilder.DropTable(
                name: "WorkshopMedia");

            migrationBuilder.DropTable(
                name: "WorkshopPaymentMode");

            migrationBuilder.DropTable(
                name: "WorkshopService");

            migrationBuilder.DropTable(
                name: "WorkshopTiming");

            migrationBuilder.DropTable(
                name: "WorkshopWorkingDay");

            migrationBuilder.DropTable(
                name: "PaymentMode");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropColumn(
                name: "AvgVehicleInflowPerMonth",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "DealerCode",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "InBusinessSince",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "IsGdprAccepted",
                table: "WorkshopProfile");

            migrationBuilder.DropColumn(
                name: "NoOfEmployees",
                table: "WorkshopProfile");

            migrationBuilder.AddColumn<long>(
                name: "WorkshopProfileId",
                table: "WorkshopUser",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalendarDate",
                table: "WorkshopProfile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_WorkshopProfileId",
                table: "WorkshopUser",
                column: "WorkshopProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopUser_WorkshopProfile_WorkshopProfileId",
                table: "WorkshopUser",
                column: "WorkshopProfileId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id");
        }
    }
}
