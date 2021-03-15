using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SellingCumputedColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceProfit",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED");
        }
    }
}
