using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SeedInstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Institution",
                columns: new[] { "InstitutionId", "Address", "City", "DialogTitle", "Email", "Established", "InstitutionLogo", "InstitutionName", "LocalArea", "Phone", "PostalCode", "State", "Website" },
                values: new object[] { 1, null, null, null, null, null, null, "Institution", null, null, null, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Institution",
                keyColumn: "InstitutionId",
                keyValue: 1);
        }
    }
}
