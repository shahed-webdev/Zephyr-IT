using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddPurchaseAmountInSellingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseDescription",
                table: "Selling",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "Selling",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Selling_PurchaseId",
                table: "Selling",
                column: "PurchaseId",
                unique: true,
                filter: "[PurchaseId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Selling_Purchase",
                table: "Selling",
                column: "PurchaseId",
                principalTable: "Purchase",
                principalColumn: "PurchaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Selling_Purchase",
                table: "Selling");

            migrationBuilder.DropIndex(
                name: "IX_Selling_PurchaseId",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "PurchaseAmount",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "PurchaseDescription",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "Selling");
        }
    }
}
