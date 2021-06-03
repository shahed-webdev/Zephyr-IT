using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SellingNetProfitWithoutDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SellingNetProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SellingNetProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED");
        }
    }
}
