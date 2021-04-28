using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddAdminMoneyCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminMoneyCollection",
                columns: table => new
                {
                    AdminMoneyCollectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    CollectionAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    InsertDateUtc = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMoneyCollection", x => x.AdminMoneyCollectionId);
                    table.ForeignKey(
                        name: "FK_AdminMoneyCollection_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminMoneyCollection_RegistrationId",
                table: "AdminMoneyCollection",
                column: "RegistrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminMoneyCollection");
        }
    }
}
