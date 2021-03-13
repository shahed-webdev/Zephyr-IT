using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class computedColumnDone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Vendor",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "ServicePaymentStatus",
                table: "Service",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when ([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceDueAmount",
                table: "Service",
                nullable: false,
                computedColumnSql: "([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceDiscountPercentage",
                table: "Service",
                nullable: false,
                computedColumnSql: "(case when [ServiceTotalPrice]=(0) then (0) else ([ServiceDiscountAmount]*(100))/[ServiceTotalPrice] end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountCost",
                table: "SellingPayment",
                nullable: false,
                computedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100)) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "PurchasePaymentStatus",
                table: "Purchase",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseDueAmount",
                table: "Purchase",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseDiscountPercentage",
                table: "Purchase",
                nullable: false,
                computedColumnSql: "(case when [PurchaseTotalPrice]=(0) then (0) else ([PurchaseDiscountAmount]*(100))/[PurchaseTotalPrice] end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPerDay",
                table: "ExpenseFixed",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([Amount]/[IntervalDays]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Customer",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Vendor",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid])) PERSISTED");

            migrationBuilder.AlterColumn<string>(
                name: "ServicePaymentStatus",
                table: "Service",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when ([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceDueAmount",
                table: "Service",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceDiscountPercentage",
                table: "Service",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(case when [ServiceTotalPrice]=(0) then (0) else ([ServiceDiscountAmount]*(100))/[ServiceTotalPrice] end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountCost",
                table: "SellingPayment",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100)) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceProfit",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "([ServiceCharge]-[ServiceCost]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED");

            migrationBuilder.AlterColumn<string>(
                name: "PurchasePaymentStatus",
                table: "Purchase",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseDueAmount",
                table: "Purchase",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount])) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseDiscountPercentage",
                table: "Purchase",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(case when [PurchaseTotalPrice]=(0) then (0) else ([PurchaseDiscountAmount]*(100))/[PurchaseTotalPrice] end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPerDay",
                table: "ExpenseFixed",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([Amount]/[IntervalDays]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Customer",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "0",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid])) PERSISTED");
        }
    }
}
