using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddNoteToProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "PurchaseList",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Product",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "PurchaseList");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Product");
        }
    }
}
