using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddAccountTransactionSellingPaymentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccountTransactionCharge",
                table: "SellingPaymentList",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "AccountTransactionCharge",
                table: "SellingPayment",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValueSql: "(0.00)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountTransactionCharge",
                table: "SellingPaymentList");

            migrationBuilder.DropColumn(
                name: "AccountTransactionCharge",
                table: "SellingPayment");
        }
    }
}
