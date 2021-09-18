using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SellingExpenseTableAccountAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "SellingExpense",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellingExpense_AccountId",
                table: "SellingExpense",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellingExpense_Account",
                table: "SellingExpense",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellingExpense_Account",
                table: "SellingExpense");

            migrationBuilder.DropIndex(
                name: "IX_SellingExpense_AccountId",
                table: "SellingExpense");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "SellingExpense");
        }
    }
}
