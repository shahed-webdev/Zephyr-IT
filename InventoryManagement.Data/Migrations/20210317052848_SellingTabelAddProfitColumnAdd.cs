using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SellingTabelAddProfitColumnAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost])) PERSISTED");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0.00) then (0.00) else ([SellingDiscountAmount]*(100.00))/[SellingTotalPrice] end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end) PERSISTED");

            migrationBuilder.AddColumn<decimal>(
                name: "GrandProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal]))+([ServiceCharge]-[ServiceCost])) PERSISTED");

            migrationBuilder.AddColumn<decimal>(
                name: "SellingNetProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrandProfit",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "SellingNetProfit",
                table: "Selling");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount])) PERSISTED");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0.00) then (0.00) else ([SellingDiscountAmount]*(100.00))/[SellingTotalPrice] end) PERSISTED");
        }
    }
}
