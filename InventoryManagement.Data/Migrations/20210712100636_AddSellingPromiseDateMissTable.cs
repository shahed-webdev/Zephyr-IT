using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddSellingPromiseDateMissTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellingPromiseDateMiss",
                columns: table => new
                {
                    SellingPromiseDateMissId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    SellingId = table.Column<int>(nullable: false),
                    MissDate = table.Column<DateTime>(type: "date", nullable: false),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingPromiseDateMiss", x => x.SellingPromiseDateMissId);
                    table.ForeignKey(
                        name: "FK_SellingPromiseDateMiss_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId");
                    table.ForeignKey(
                        name: "FK_SellingPromiseDateMiss_Selling",
                        column: x => x.SellingId,
                        principalTable: "Selling",
                        principalColumn: "SellingId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellingPromiseDateMiss_RegistrationId",
                table: "SellingPromiseDateMiss",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPromiseDateMiss_SellingId",
                table: "SellingPromiseDateMiss",
                column: "SellingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellingPromiseDateMiss");
        }
    }
}
