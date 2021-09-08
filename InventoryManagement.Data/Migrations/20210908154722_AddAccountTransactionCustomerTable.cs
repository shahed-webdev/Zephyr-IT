using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddAccountTransactionCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccountTransactionCharge",
                table: "Customer",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Customer",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount]+[AccountTransactionCharge])-([TotalDiscount]+[Paid]+[PurchaseAdjustedAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]+[PurchaseAdjustedAmount])) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountTransactionCharge",
                table: "Customer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Customer",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]+[PurchaseAdjustedAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount]+[AccountTransactionCharge])-([TotalDiscount]+[Paid]+[PurchaseAdjustedAmount])) PERSISTED");
        }
    }
}
