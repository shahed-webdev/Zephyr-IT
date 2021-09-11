using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddPurchasePaymentReturnRecordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasePaymentReturnRecord",
                columns: table => new
                {
                    PurchasePaymentReturnRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrevReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CurrentReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    NetReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false, computedColumnSql: "([PrevReturnAmount]-[CurrentReturnAmount]) PERSISTED"),
                    AccountId = table.Column<int>(nullable: true),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    PurchaseId = table.Column<int>(nullable: false),
                    RegistrationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePaymentReturnRecord", x => x.PurchasePaymentReturnRecordId);
                    table.ForeignKey(
                        name: "FK_PurchasePaymentReturnRecord_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasePaymentReturnRecord_Purchase",
                        column: x => x.PurchaseId,
                        principalTable: "Purchase",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasePaymentReturnRecord_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePaymentReturnRecord_AccountId",
                table: "PurchasePaymentReturnRecord",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePaymentReturnRecord_PurchaseId",
                table: "PurchasePaymentReturnRecord",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePaymentReturnRecord_RegistrationId",
                table: "PurchasePaymentReturnRecord",
                column: "RegistrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasePaymentReturnRecord");
        }
    }
}
