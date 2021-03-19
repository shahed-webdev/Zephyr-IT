using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddAccountInPurchasePayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "PurchasePayment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayment_AccountId",
                table: "PurchasePayment",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePayment_Account",
                table: "PurchasePayment",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePayment_Account",
                table: "PurchasePayment");

            migrationBuilder.DropIndex(
                name: "IX_PurchasePayment_AccountId",
                table: "PurchasePayment");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "PurchasePayment");
        }
    }
}
