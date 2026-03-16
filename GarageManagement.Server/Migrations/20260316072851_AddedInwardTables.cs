using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedInwardTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InwardNote",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InwardNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchaseOrderId = table.Column<long>(type: "bigint", nullable: true),
                    JobCardId = table.Column<long>(type: "bigint", nullable: true),
                    RegNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobCardNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryReceipt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false),
                    FreightAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TcsAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InwardNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InwardNote_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InwardNoteItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InwardNoteId = table.Column<long>(type: "bigint", nullable: false),
                    PartId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseOrderItemId = table.Column<long>(type: "bigint", nullable: true),
                    InwardQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TaxPercent = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalPurchasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Margin = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    RackNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: true),
                    RowState = table.Column<byte>(type: "tinyint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InwardNoteItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InwardNoteItem_InwardNote_InwardNoteId",
                        column: x => x.InwardNoteId,
                        principalTable: "InwardNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InwardNote_SupplierId",
                table: "InwardNote",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InwardNote_WorkshopId_InwardNo",
                table: "InwardNote",
                columns: new[] { "WorkshopId", "InwardNo" },
                unique: true,
                filter: "[WorkshopId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InwardNoteItem_InwardNoteId",
                table: "InwardNoteItem",
                column: "InwardNoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InwardNoteItem");

            migrationBuilder.DropTable(
                name: "InwardNote");
        }
    }
}
