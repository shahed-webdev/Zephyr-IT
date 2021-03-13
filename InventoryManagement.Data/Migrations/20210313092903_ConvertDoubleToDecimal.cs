using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class ConvertDoubleToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscount",
                table: "Vendor",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Vendor",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnAmount",
                table: "Vendor",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Paid",
                table: "Vendor",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePaidAmount",
                table: "ServicePaymentList",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceCharge",
                table: "ServiceList",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceTotalPrice",
                table: "Service",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePaidAmount",
                table: "Service",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceDiscountAmount",
                table: "Service",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPaidAmount",
                table: "SellingPaymentList",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "SellingPayment",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "SellingList",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "SellingList",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingTotalPrice",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingReturnAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPaidAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePaidAmount",
                table: "PurchasePaymentList",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "PurchasePayment",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "PurchaseList",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "PurchaseList",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseTotalPrice",
                table: "Purchase",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseReturnAmount",
                table: "Purchase",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePaidAmount",
                table: "Purchase",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseDiscountAmount",
                table: "Purchase",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "Product",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpenseAmount",
                table: "ExpenseTransportationList",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalExpense",
                table: "ExpenseTransportation",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ExpenseFixed",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpenseAmount",
                table: "Expense",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscount",
                table: "Customer",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Customer",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnAmount",
                table: "Customer",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Paid",
                table: "Customer",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "DueLimit",
                table: "Customer",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalDiscount",
                table: "Vendor",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "TotalAmount",
                table: "Vendor",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ReturnAmount",
                table: "Vendor",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "Paid",
                table: "Vendor",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ServicePaidAmount",
                table: "ServicePaymentList",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceCharge",
                table: "ServiceList",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceTotalPrice",
                table: "Service",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ServicePaidAmount",
                table: "Service",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ServiceDiscountAmount",
                table: "Service",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPaidAmount",
                table: "SellingPaymentList",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "PaidAmount",
                table: "SellingPayment",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "SellingList",
                type: "float",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PurchasePrice",
                table: "SellingList",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingTotalPrice",
                table: "Selling",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingReturnAmount",
                table: "Selling",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPaidAmount",
                table: "Selling",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingDiscountAmount",
                table: "Selling",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "PurchasePaidAmount",
                table: "PurchasePaymentList",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "PaidAmount",
                table: "PurchasePayment",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "PurchaseList",
                type: "float",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PurchasePrice",
                table: "PurchaseList",
                type: "float",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PurchaseTotalPrice",
                table: "Purchase",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "PurchaseReturnAmount",
                table: "Purchase",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "PurchasePaidAmount",
                table: "Purchase",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "PurchaseDiscountAmount",
                table: "Purchase",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "Product",
                type: "float",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "ExpenseAmount",
                table: "ExpenseTransportationList",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "TotalExpense",
                table: "ExpenseTransportation",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "ExpenseFixed",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ExpenseAmount",
                table: "Expense",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "TotalDiscount",
                table: "Customer",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "TotalAmount",
                table: "Customer",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "ReturnAmount",
                table: "Customer",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "Paid",
                table: "Customer",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<double>(
                name: "DueLimit",
                table: "Customer",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }
    }
}
