using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddSellingPurchaseInProductLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "ProductLog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SellingId",
                table: "ProductLog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductLog_PurchaseId",
                table: "ProductLog",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLog_SellingId",
                table: "ProductLog",
                column: "SellingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLog_Purchase",
                table: "ProductLog",
                column: "PurchaseId",
                principalTable: "Purchase",
                principalColumn: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLog_Selling",
                table: "ProductLog",
                column: "SellingId",
                principalTable: "Selling",
                principalColumn: "SellingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLog_Purchase",
                table: "ProductLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductLog_Selling",
                table: "ProductLog");

            migrationBuilder.DropIndex(
                name: "IX_ProductLog_PurchaseId",
                table: "ProductLog");

            migrationBuilder.DropIndex(
                name: "IX_ProductLog_SellingId",
                table: "ProductLog");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "ProductLog");

            migrationBuilder.DropColumn(
                name: "SellingId",
                table: "ProductLog");
        }
    }
}
