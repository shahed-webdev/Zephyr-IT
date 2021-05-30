using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class PurchaseAdjustedAmountInDueColumnInSellingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount])) PERSISTED");
        }
    }
}
