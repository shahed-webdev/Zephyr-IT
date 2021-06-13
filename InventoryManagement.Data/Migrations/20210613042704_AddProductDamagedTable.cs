using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddProductDamagedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductDamaged",
                columns: table => new
                {
                    ProductDamagedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductStockId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(maxLength: 1024, nullable: true),
                    DamagedDate = table.Column<DateTime>(type: "date", nullable: false),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDamaged", x => x.ProductDamagedId);
                    table.ForeignKey(
                        name: "FK_ProductDamaged_ProductStock",
                        column: x => x.ProductStockId,
                        principalTable: "ProductStock",
                        principalColumn: "ProductStockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDamaged_ProductStockId",
                table: "ProductDamaged",
                column: "ProductStockId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDamaged");
        }
    }
}
