using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddPurchaseAdjustedAmountInCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseAdjustedAmount",
                table: "Customer",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Customer",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]+[PurchaseAdjustedAmount])) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid])) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseAdjustedAmount",
                table: "Customer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Due",
                table: "Customer",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid])) PERSISTED",
                oldClrType: typeof(decimal),
                oldComputedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]+[PurchaseAdjustedAmount])) PERSISTED");
        }
    }
}
