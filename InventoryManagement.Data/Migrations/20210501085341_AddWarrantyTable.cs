using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddWarrantyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Warranty",
                columns: table => new
                {
                    WarrantyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellingId = table.Column<int>(nullable: false),
                    ProductStockId = table.Column<int>(nullable: false),
                    ChangedProductCatalogId = table.Column<int>(nullable: false),
                    WarrantySn = table.Column<int>(nullable: false),
                    AcceptanceDescription = table.Column<string>(maxLength: 1024, nullable: true),
                    AcceptanceDate = table.Column<DateTime>(type: "date", nullable: false),
                    DeliveryDescription = table.Column<string>(maxLength: 1024, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "date", nullable: true),
                    ChangedProductName = table.Column<string>(maxLength: 128, nullable: true),
                    ChangedProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsDelivered = table.Column<bool>(nullable: false),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warranty", x => x.WarrantyId);
                    table.ForeignKey(
                        name: "FK_Warranty_ProductCatalog",
                        column: x => x.ChangedProductCatalogId,
                        principalTable: "ProductCatalog",
                        principalColumn: "ProductCatalogId");
                    table.ForeignKey(
                        name: "FK_Warranty_ProductStock",
                        column: x => x.ProductStockId,
                        principalTable: "ProductStock",
                        principalColumn: "ProductStockId");
                    table.ForeignKey(
                        name: "FK_Warranty_Selling",
                        column: x => x.SellingId,
                        principalTable: "Selling",
                        principalColumn: "SellingId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_ChangedProductCatalogId",
                table: "Warranty",
                column: "ChangedProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_ProductStockId",
                table: "Warranty",
                column: "ProductStockId");

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_SellingId",
                table: "Warranty",
                column: "SellingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Warranty");
        }
    }
}
