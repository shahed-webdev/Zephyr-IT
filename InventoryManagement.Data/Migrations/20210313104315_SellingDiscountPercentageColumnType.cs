using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SellingDiscountPercentageColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SellingDiscountPercentage",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end) PERSISTED");
        }
    }
}
