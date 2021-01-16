using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddExpenseTransportationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseTransportation",
                columns: table => new
                {
                    ExpenseTransportationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    VoucherNo = table.Column<int>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    RegistrationId = table.Column<int>(nullable: false),
                    TotalExpense = table.Column<double>(nullable: false),
                    ExpenseNote = table.Column<string>(maxLength: 500, nullable: true),
                    ExpenseDate = table.Column<DateTime>(type: "date", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTransportation", x => x.ExpenseTransportationId);
                    table.ForeignKey(
                        name: "FK_ExpenseTransportation_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_ExpenseTransportation_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTransportationList",
                columns: table => new
                {
                    ExpenseTransportationListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseTransportationId = table.Column<int>(nullable: false),
                    NumberOfPerson = table.Column<int>(nullable: false),
                    ExpenseFor = table.Column<string>(maxLength: 256, nullable: false),
                    Vehicle = table.Column<string>(maxLength: 128, nullable: false),
                    ExpenseAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTransportationList", x => x.ExpenseTransportationListId);
                    table.ForeignKey(
                        name: "FK_ExpenseTransportationList_ExpenseTransportation_ExpenseTransportationId",
                        column: x => x.ExpenseTransportationId,
                        principalTable: "ExpenseTransportation",
                        principalColumn: "ExpenseTransportationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransportation_CustomerId",
                table: "ExpenseTransportation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransportation_RegistrationId",
                table: "ExpenseTransportation",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransportationList_ExpenseTransportationId",
                table: "ExpenseTransportationList",
                column: "ExpenseTransportationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseTransportationList");

            migrationBuilder.DropTable(
                name: "ExpenseTransportation");
        }
    }
}
