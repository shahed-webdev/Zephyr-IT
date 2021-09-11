using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class ReturnAmountNetUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NetReturnAmount",
                table: "SellingPaymentReturnRecord",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([CurrentReturnAmount]-[PrevReturnAmount]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([PrevReturnAmount]-[CurrentReturnAmount]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetReturnAmount",
                table: "PurchasePaymentReturnRecord",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([CurrentReturnAmount]-[PrevReturnAmount]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([PrevReturnAmount]-[CurrentReturnAmount]) PERSISTED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NetReturnAmount",
                table: "SellingPaymentReturnRecord",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([PrevReturnAmount]-[CurrentReturnAmount]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([CurrentReturnAmount]-[PrevReturnAmount]) PERSISTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetReturnAmount",
                table: "PurchasePaymentReturnRecord",
                type: "decimal(18, 2)",
                nullable: false,
                computedColumnSql: "([PrevReturnAmount]-[CurrentReturnAmount]) PERSISTED",
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldComputedColumnSql: "([CurrentReturnAmount]-[PrevReturnAmount]) PERSISTED");
        }
    }
}
