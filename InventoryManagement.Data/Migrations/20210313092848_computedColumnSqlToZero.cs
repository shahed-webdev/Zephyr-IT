using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class computedColumnSqlToZero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Vendor",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))");

            migrationBuilder.AlterColumn<string>(
                name: "ServicePaymentStatus",
                table: "Service",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when ([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]))<=(0) then 'Paid' else 'Due' end)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceDueAmount",
                table: "Service",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(round([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]),(2)))");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceDiscountPercentage",
                table: "Service",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(case when [ServiceTotalPrice]=(0) then (0) else round(([ServiceDiscountAmount]*(100))/[ServiceTotalPrice],(2)) end)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountCost",
                table: "SellingPayment",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100)) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(round(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end)");

            migrationBuilder.AlterColumn<string>(
                name: "PurchasePaymentStatus",
                table: "Purchase",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]))<=(0) then 'Paid' else 'Due' end)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseDueAmount",
                table: "Purchase",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(round(([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]),(2)))");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseDiscountPercentage",
                table: "Purchase",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(case when [PurchaseTotalPrice]=(0) then (0) else round(([PurchaseDiscountAmount]*(100))/[PurchaseTotalPrice],(2)) end)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPerDay",
                table: "ExpenseFixed",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "ROUND(([Amount]/[IntervalDays]),2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Customer",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<double>(
                name: "Due",
                table: "Vendor",
                type: "float",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "ServicePaymentStatus",
                table: "Service",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when ([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]))<=(0) then 'Paid' else 'Due' end)",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceDueAmount",
                table: "Service",
                type: "float",
                nullable: false,
                computedColumnSql: "(round([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]),(2)))",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceDiscountPercentage",
                table: "Service",
                type: "float",
                nullable: false,
                computedColumnSql: "(case when [ServiceTotalPrice]=(0) then (0) else round(([ServiceDiscountAmount]*(100))/[ServiceTotalPrice],(2)) end)",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountCost",
                table: "SellingPayment",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100)) PERSISTED",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceProfit",
                table: "Selling",
                type: "float",
                nullable: false,
                computedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "float",
                nullable: false,
                computedColumnSql: "(round(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "float",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end)",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "PurchasePaymentStatus",
                table: "Purchase",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]))<=(0) then 'Paid' else 'Due' end)",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PurchaseDueAmount",
                table: "Purchase",
                type: "float",
                nullable: false,
                computedColumnSql: "(round(([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]),(2)))",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PurchaseDiscountPercentage",
                table: "Purchase",
                type: "float",
                nullable: false,
                computedColumnSql: "(case when [PurchaseTotalPrice]=(0) then (0) else round(([PurchaseDiscountAmount]*(100))/[PurchaseTotalPrice],(2)) end)",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "CostPerDay",
                table: "ExpenseFixed",
                type: "float",
                nullable: false,
                computedColumnSql: "ROUND(([Amount]/[IntervalDays]),2)",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "Due",
                table: "Customer",
                type: "float",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "0");
        }
    }
}
