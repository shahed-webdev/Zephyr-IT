using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddServiceChargeToSellingDueColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SellingPaymentStatus",
                table: "Selling",
                unicode: false,
                maxLength: 4,
                nullable: false,
                computedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)",
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingDueAmount",
                table: "Selling",
                nullable: false,
                computedColumnSql: "(round(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))",
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "(round(([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))");
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
                computedColumnSql: "(case when (([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4,
                oldComputedColumnSql: "(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)");

            migrationBuilder.AlterColumn<double>(
                name: "SellingDueAmount",
                table: "Selling",
                type: "float",
                nullable: false,
                computedColumnSql: "(round(([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))",
                oldClrType: typeof(double),
                oldComputedColumnSql: "(round(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))");
        }
    }
}
