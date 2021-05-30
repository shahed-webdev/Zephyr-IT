using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class ChangePurchaseAdjustedAmountInSellingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseAmount",
                table: "Selling",
                newName: "PurchaseAdjustedAmount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseAdjustedAmount",
                table: "Selling",
                newName: "PurchaseAmount");
        }
    }
}
