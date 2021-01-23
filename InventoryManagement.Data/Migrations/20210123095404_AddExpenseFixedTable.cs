using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddExpenseFixedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseFixed",
                columns: table => new
                {
                    ExpenseFixedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    IntervalDays = table.Column<int>(nullable: false),
                    CostPerDay = table.Column<double>(nullable: false, computedColumnSql: "ROUND(([Amount]/[IntervalDays]),2)"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseFixed", x => x.ExpenseFixedId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseFixed");
        }
    }
}
