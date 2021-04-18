using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddRoleSalesPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "F73A5277-2535-48A4-A371-300508ADDD2F",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "SubAdmin", "SUBADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "95A97547-7B72-4E5C-855C-AA1F8CA327E8", "95A97547-7B72-4E5C-855C-AA1F8CA327E8", "SalesPerson", "SALESPERSON" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95A97547-7B72-4E5C-855C-AA1F8CA327E8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "F73A5277-2535-48A4-A371-300508ADDD2F",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "sub-admin", "SUB-ADMIN" });
        }
    }
}
