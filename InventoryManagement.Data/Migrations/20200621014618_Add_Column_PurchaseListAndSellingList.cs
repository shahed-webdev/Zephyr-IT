using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class Add_Column_PurchaseListAndSellingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SellingList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Warranty",
                table: "SellingList",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PurchaseList",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SellingPrice",
                table: "PurchaseList",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Warranty",
                table: "PurchaseList",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SellingList");

            migrationBuilder.DropColumn(
                name: "Warranty",
                table: "SellingList");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PurchaseList");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "PurchaseList");

            migrationBuilder.DropColumn(
                name: "Warranty",
                table: "PurchaseList");
        }
    }
}
