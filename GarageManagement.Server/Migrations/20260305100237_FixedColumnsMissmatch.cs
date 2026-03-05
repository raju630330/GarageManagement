using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixedColumnsMissmatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopPaymentMode_WorkshopProfile_WorkshopId",
                table: "WorkshopPaymentMode");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopService_WorkshopProfile_WorkshopId",
                table: "WorkshopService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkshopService",
                table: "WorkshopService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkshopPaymentMode",
                table: "WorkshopPaymentMode");

            migrationBuilder.AlterColumn<long>(
                name: "WorkshopId",
                table: "WorkshopService",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "WorkshopId",
                table: "WorkshopPaymentMode",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkshopService",
                table: "WorkshopService",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkshopPaymentMode",
                table: "WorkshopPaymentMode",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopService_WorkshopId",
                table: "WorkshopService",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopPaymentMode_WorkshopId",
                table: "WorkshopPaymentMode",
                column: "WorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopPaymentMode_WorkshopProfile_WorkshopId",
                table: "WorkshopPaymentMode",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopService_WorkshopProfile_WorkshopId",
                table: "WorkshopService",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopPaymentMode_WorkshopProfile_WorkshopId",
                table: "WorkshopPaymentMode");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkshopService_WorkshopProfile_WorkshopId",
                table: "WorkshopService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkshopService",
                table: "WorkshopService");

            migrationBuilder.DropIndex(
                name: "IX_WorkshopService_WorkshopId",
                table: "WorkshopService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkshopPaymentMode",
                table: "WorkshopPaymentMode");

            migrationBuilder.DropIndex(
                name: "IX_WorkshopPaymentMode_WorkshopId",
                table: "WorkshopPaymentMode");

            migrationBuilder.AlterColumn<long>(
                name: "WorkshopId",
                table: "WorkshopService",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "WorkshopId",
                table: "WorkshopPaymentMode",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkshopService",
                table: "WorkshopService",
                columns: new[] { "WorkshopId", "ServiceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkshopPaymentMode",
                table: "WorkshopPaymentMode",
                columns: new[] { "WorkshopId", "PaymentModeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopPaymentMode_WorkshopProfile_WorkshopId",
                table: "WorkshopPaymentMode",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkshopService_WorkshopProfile_WorkshopId",
                table: "WorkshopService",
                column: "WorkshopId",
                principalTable: "WorkshopProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
