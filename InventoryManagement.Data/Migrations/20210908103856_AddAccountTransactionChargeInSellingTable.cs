using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddAccountTransactionChargeInSellingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccountTransactionCharge",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount]+[AccountTransactionCharge])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingNetProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[AccountTransactionCharge])-([BuyingTotalPrice]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount]+[AccountTransactionCharge])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrandProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "((([SellingTotalPrice]+[AccountTransactionCharge])-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal]))+([ServiceCharge]-[ServiceCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal]))+([ServiceCharge]-[ServiceCost])) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountTransactionCharge",
                table: "Selling");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount]+[AccountTransactionCharge])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingNetProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([SellingTotalPrice]-([BuyingTotalPrice]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([SellingTotalPrice]+[AccountTransactionCharge])-([BuyingTotalPrice]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount]+[AccountTransactionCharge])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrandProfit",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal]))+([ServiceCharge]-[ServiceCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "((([SellingTotalPrice]+[AccountTransactionCharge])-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal]))+([ServiceCharge]-[ServiceCost])) PERSISTED");
        }
    }
}
