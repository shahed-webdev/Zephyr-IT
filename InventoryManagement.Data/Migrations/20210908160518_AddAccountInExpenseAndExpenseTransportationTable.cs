using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddAccountInExpenseAndExpenseTransportationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "ExpenseTransportation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Expense",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransportation_AccountId",
                table: "ExpenseTransportation",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_AccountId",
                table: "Expense",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Account",
                table: "Expense",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseTransportation_Account",
                table: "ExpenseTransportation",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Account",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseTransportation_Account",
                table: "ExpenseTransportation");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseTransportation_AccountId",
                table: "ExpenseTransportation");

            migrationBuilder.DropIndex(
                name: "IX_Expense_AccountId",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "ExpenseTransportation");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Expense");
        }
    }
}
