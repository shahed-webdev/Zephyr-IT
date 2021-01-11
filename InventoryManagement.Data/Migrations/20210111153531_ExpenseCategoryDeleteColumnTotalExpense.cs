using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class ExpenseCategoryDeleteColumnTotalExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalExpense",
                table: "ExpenseCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalExpense",
                table: "ExpenseCategory",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
