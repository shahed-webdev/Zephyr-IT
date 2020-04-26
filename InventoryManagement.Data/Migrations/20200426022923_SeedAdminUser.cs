using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class SeedAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5A71C6C4-9488-4BCC-A680-445A34C6E721", "5A71C6C4-9488-4BCC-A680-445A34C6E721", "admin", "ADMIN" },
                    { "F73A5277-2535-48A4-A371-300508ADDD2F", "F73A5277-2535-48A4-A371-300508ADDD2F", "sub-admin", "SUB-ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "A0456563-F978-4135-B563-97F23EA02FDA", 0, "A0456563-F978-4135-B563-97F23EA02FDA", "admin@gmail.com", true, true, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEDch3arYEB9dCAudNdsYEpVX7ryywa8f3ZIJSVUmEThAI50pLh9RyEu7NjGJccpOog==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "Registration",
                columns: new[] { "RegistrationId", "Address", "DateofBirth", "Designation", "Email", "FatherName", "Image", "Name", "NationalID", "Phone", "PS", "Type", "UserName" },
                values: new object[] { 1, null, null, null, null, null, null, "Admin", null, null, "Admin_121", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "A0456563-F978-4135-B563-97F23EA02FDA", "5A71C6C4-9488-4BCC-A680-445A34C6E721" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "F73A5277-2535-48A4-A371-300508ADDD2F");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "A0456563-F978-4135-B563-97F23EA02FDA", "5A71C6C4-9488-4BCC-A680-445A34C6E721" });

            migrationBuilder.DeleteData(
                table: "Registration",
                keyColumn: "RegistrationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5A71C6C4-9488-4BCC-A680-445A34C6E721");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "A0456563-F978-4135-B563-97F23EA02FDA");
        }
    }
}
