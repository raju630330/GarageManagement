using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class Intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Part",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RackNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Part", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMode",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionModule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionModule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InBusinessSince = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvgVehicleInflowPerMonth = table.Column<int>(type: "int", nullable: false),
                    NoOfEmployees = table.Column<int>(type: "int", nullable: false),
                    DealerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGdprAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentWorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopProfile_WorkshopProfile_ParentWorkshopId",
                        column: x => x.ParentWorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    RegPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionModuleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_PermissionModule_PermissionModuleId",
                        column: x => x.PermissionModuleId,
                        principalTable: "PermissionModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpiresUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopAddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlatNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landmark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopAddress_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkshopBusinessConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebsiteLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleReviewLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalIntegrationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSTIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MSME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SACPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvoiceCaption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopBusinessConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopBusinessConfig_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkshopMedia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopMedia_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkshopPaymentMode",
                columns: table => new
                {
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "WorkshopTiming",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopTiming", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopTiming_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkshopWorkingDay",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopWorkingDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopWorkingDay_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookAppointment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceAdvisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAppointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookAppointment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookAppointment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookAppointment_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookAppointment_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkshopUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkshopUser_WorkshopProfile_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "WorkshopProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VinNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mkls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleSite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteInchargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnderWarranty = table.Column<bool>(type: "bit", nullable: false),
                    ExpectedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllottedTechnician = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingAppointmentId = table.Column<long>(type: "bigint", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepairEstimationCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DriverPermanetToThisVehicle = table.Column<bool>(type: "bit", nullable: false),
                    TypeOfService = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoadTestAlongWithDriver = table.Column<bool>(type: "bit", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrder_BookAppointment_BookingAppointmentId",
                        column: x => x.BookingAppointmentId,
                        principalTable: "BookAppointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalJobObserveDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalJobObserveDetail_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryForm_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCard",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobCardNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OdometerIn = table.Column<long>(type: "bigint", nullable: false),
                    AvgKmsPerDay = table.Column<long>(type: "bigint", nullable: false),
                    Vin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceAdvisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Technician = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Corporate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternateMobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoundOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceSuggestions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCard_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabourDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabourCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutsideLabour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabourDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabourDetail_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SparePartsIssueDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartsIssueDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartsIssueDetail_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianMC",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnicianSign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverSign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloorSign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthSign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianMC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicianMC_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToBeFilledBySupervisor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverVoice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorInstructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToBeFilledBySupervisor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToBeFilledBySupervisor_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAccessory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false),
                    InventoryFormId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAccessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAccessory_InventoryForm_InventoryFormId",
                        column: x => x.InventoryFormId,
                        principalTable: "InventoryForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCardAdvancePayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: false),
                    Cash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardAdvancePayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardAdvancePayment_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCardCancelledInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardCancelledInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardCancelledInvoice_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCardCollection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardCollection_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCardConcern",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardConcern", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardConcern_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCardEstimationItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: false),
                    PartId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IssuedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardEstimationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardEstimationItem_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobCardEstimationItem_Part_PartId",
                        column: x => x.PartId,
                        principalTable: "Part",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCardTyreBattery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardTyreBattery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardTyreBattery_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicianMCId = table.Column<long>(type: "bigint", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Control = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckItem_TechnicianMC_TechnicianMCId",
                        column: x => x.TechnicianMCId,
                        principalTable: "TechnicianMC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMovement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartId = table.Column<long>(type: "bigint", nullable: false),
                    EstimationItemId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovement_JobCardEstimationItem_EstimationItemId",
                        column: x => x.EstimationItemId,
                        principalTable: "JobCardEstimationItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovement_Part_PartId",
                        column: x => x.PartId,
                        principalTable: "Part",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovement_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalJobObserveDetail_RepairOrderId",
                table: "AdditionalJobObserveDetail",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_CustomerId",
                table: "BookAppointment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_UserId",
                table: "BookAppointment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_VehicleId",
                table: "BookAppointment",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAppointment_WorkshopId",
                table: "BookAppointment",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckItem_TechnicianMCId",
                table: "CheckItem",
                column: "TechnicianMCId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccessory_InventoryFormId",
                table: "InventoryAccessory",
                column: "InventoryFormId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryForm_RepairOrderId",
                table: "InventoryForm",
                column: "RepairOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCard_RepairOrderId",
                table: "JobCard",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardAdvancePayment_JobCardId",
                table: "JobCardAdvancePayment",
                column: "JobCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCardCancelledInvoice_JobCardId",
                table: "JobCardCancelledInvoice",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardCollection_JobCardId",
                table: "JobCardCollection",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardConcern_JobCardId",
                table: "JobCardConcern",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardEstimationItem_JobCardId",
                table: "JobCardEstimationItem",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardEstimationItem_PartId",
                table: "JobCardEstimationItem",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTyreBattery_JobCardId",
                table: "JobCardTyreBattery",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_LabourDetail_RepairOrderId",
                table: "LabourDetail",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Part_PartNo",
                table: "Part",
                column: "PartNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrder_BookingAppointmentId",
                table: "RepairOrder",
                column: "BookingAppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionModuleId",
                table: "RolePermission",
                column: "PermissionModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartsIssueDetail_RepairOrderId",
                table: "SparePartsIssueDetail",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_EstimationItemId",
                table: "StockMovement",
                column: "EstimationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_PartId",
                table: "StockMovement",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_UserId",
                table: "StockMovement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianMC_RepairOrderId",
                table: "TechnicianMC",
                column: "RepairOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToBeFilledBySupervisor_RepairOrderId",
                table: "ToBeFilledBySupervisor",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CustomerId",
                table: "Vehicle",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopAddress_WorkshopId",
                table: "WorkshopAddress",
                column: "WorkshopId",
                unique: true,
                filter: "[WorkshopId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopBusinessConfig_WorkshopId",
                table: "WorkshopBusinessConfig",
                column: "WorkshopId",
                unique: true,
                filter: "[WorkshopId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopMedia_WorkshopId",
                table: "WorkshopMedia",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopPaymentMode_PaymentModeId",
                table: "WorkshopPaymentMode",
                column: "PaymentModeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopProfile_ParentWorkshopId",
                table: "WorkshopProfile",
                column: "ParentWorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopService_ServiceId",
                table: "WorkshopService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopTiming_WorkshopId",
                table: "WorkshopTiming",
                column: "WorkshopId",
                unique: true,
                filter: "[WorkshopId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_UserId",
                table: "WorkshopUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopUser_WorkshopId",
                table: "WorkshopUser",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopWorkingDay_WorkshopId",
                table: "WorkshopWorkingDay",
                column: "WorkshopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalJobObserveDetail");

            migrationBuilder.DropTable(
                name: "CheckItem");

            migrationBuilder.DropTable(
                name: "InventoryAccessory");

            migrationBuilder.DropTable(
                name: "JobCardAdvancePayment");

            migrationBuilder.DropTable(
                name: "JobCardCancelledInvoice");

            migrationBuilder.DropTable(
                name: "JobCardCollection");

            migrationBuilder.DropTable(
                name: "JobCardConcern");

            migrationBuilder.DropTable(
                name: "JobCardTyreBattery");

            migrationBuilder.DropTable(
                name: "LabourDetail");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "SparePartsIssueDetail");

            migrationBuilder.DropTable(
                name: "StockMovement");

            migrationBuilder.DropTable(
                name: "ToBeFilledBySupervisor");

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
                name: "WorkshopUser");

            migrationBuilder.DropTable(
                name: "WorkshopWorkingDay");

            migrationBuilder.DropTable(
                name: "TechnicianMC");

            migrationBuilder.DropTable(
                name: "InventoryForm");

            migrationBuilder.DropTable(
                name: "PermissionModule");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "JobCardEstimationItem");

            migrationBuilder.DropTable(
                name: "PaymentMode");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "JobCard");

            migrationBuilder.DropTable(
                name: "Part");

            migrationBuilder.DropTable(
                name: "RepairOrder");

            migrationBuilder.DropTable(
                name: "BookAppointment");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "WorkshopProfile");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
