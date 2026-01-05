using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleName",
                table: "RolePermission");

            migrationBuilder.AlterColumn<long>(
                name: "PermissionId",
                table: "RolePermission",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "PermissionModuleId",
                table: "RolePermission",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PermissionModule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionModule", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionModuleId",
                table: "RolePermission",
                column: "PermissionModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_PermissionModule_PermissionModuleId",
                table: "RolePermission",
                column: "PermissionModuleId",
                principalTable: "PermissionModule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_PermissionModule_PermissionModuleId",
                table: "RolePermission");

            migrationBuilder.DropTable(
                name: "PermissionModule");

            migrationBuilder.DropIndex(
                name: "IX_RolePermission_PermissionModuleId",
                table: "RolePermission");

            migrationBuilder.DropColumn(
                name: "PermissionModuleId",
                table: "RolePermission");

            migrationBuilder.AlterColumn<long>(
                name: "PermissionId",
                table: "RolePermission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                table: "RolePermission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
