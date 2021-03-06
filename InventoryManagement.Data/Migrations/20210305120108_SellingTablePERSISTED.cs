using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SellingTablePERSISTED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AccountCost",
                table: "SellingPayment",
                nullable: false,
                computedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100)) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100))");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "([ServiceCharge]-[ServiceCost])");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost]))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AccountCost",
                table: "SellingPayment",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100))",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100)) PERSISTED");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceProfit",
                table: "Selling",
                type: "float",
                nullable: false,
                computedColumnSql: "([ServiceCharge]-[ServiceCost])",
                oldClrType: typeof(double),
                oldComputedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost]))",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED");
        }
    }
}
