using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddProductLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductLog",
                columns: table => new
                {
                    ProductLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductStockId = table.Column<int>(nullable: false),
                    Details = table.Column<string>(maxLength: 1024, nullable: false),
                    LogStatus = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ActivityByRegistrationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLog", x => x.ProductLogId);
                    table.ForeignKey(
                        name: "FK_ProductLog_Registration",
                        column: x => x.ActivityByRegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId");
                    table.ForeignKey(
                        name: "FK_ProductLog_ProductStock",
                        column: x => x.ProductStockId,
                        principalTable: "ProductStock",
                        principalColumn: "ProductStockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductLog_ActivityByRegistrationId",
                table: "ProductLog",
                column: "ActivityByRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLog_ProductStockId",
                table: "ProductLog",
                column: "ProductStockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductLog");
        }
    }
}
