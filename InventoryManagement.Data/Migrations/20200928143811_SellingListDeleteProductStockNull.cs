using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SellingListDeleteProductStockNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_SellingList",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_SellingList_Selling",
                table: "SellingList");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_SellingList",
                table: "ProductStock",
                column: "SellingListId",
                principalTable: "SellingList",
                principalColumn: "SellingListId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SellingList_Selling",
                table: "SellingList",
                column: "SellingId",
                principalTable: "Selling",
                principalColumn: "SellingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_SellingList",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_SellingList_Selling",
                table: "SellingList");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_SellingList",
                table: "ProductStock",
                column: "SellingListId",
                principalTable: "SellingList",
                principalColumn: "SellingListId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellingList_Selling",
                table: "SellingList",
                column: "SellingId",
                principalTable: "Selling",
                principalColumn: "SellingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
