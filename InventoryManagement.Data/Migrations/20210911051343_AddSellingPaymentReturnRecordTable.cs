using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddSellingPaymentReturnRecordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellingPaymentReturnRecord",
                columns: table => new
                {
                    SellingPaymentReturnRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrevReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CurrentReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    NetReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false, computedColumnSql: "([PrevReturnAmount]-[CurrentReturnAmount]) PERSISTED"),
                    AccountId = table.Column<int>(nullable: true),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    SellingId = table.Column<int>(nullable: false),
                    RegistrationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingPaymentReturnRecord", x => x.SellingPaymentReturnRecordId);
                    table.ForeignKey(
                        name: "FK_SellingPaymentReturnRecord_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingPaymentReturnRecord_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingPaymentReturnRecord_Selling",
                        column: x => x.SellingId,
                        principalTable: "Selling",
                        principalColumn: "SellingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Institution_DefaultAccountId",
                table: "Institution",
                column: "DefaultAccountId",
                unique: true,
                filter: "[DefaultAccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPaymentReturnRecord_AccountId",
                table: "SellingPaymentReturnRecord",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPaymentReturnRecord_RegistrationId",
                table: "SellingPaymentReturnRecord",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPaymentReturnRecord_SellingId",
                table: "SellingPaymentReturnRecord",
                column: "SellingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_Account_DefaultAccountId",
                table: "Institution",
                column: "DefaultAccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_Account_DefaultAccountId",
                table: "Institution");

            migrationBuilder.DropTable(
                name: "SellingPaymentReturnRecord");

            migrationBuilder.DropIndex(
                name: "IX_Institution_DefaultAccountId",
                table: "Institution");
        }
    }
}
