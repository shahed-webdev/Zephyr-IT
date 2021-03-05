using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddAccountTablesUpdatesSellingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccountCostPercentage",
                table: "SellingPayment",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "SellingPayment",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PurchasePrice",
                table: "SellingList",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "BuyingTotalPrice",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Expense",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ExpenseDescription",
                table: "Selling",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingAccountCost",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCharge",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ServiceChargeDescription",
                table: "Selling",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCost",
                table: "Selling",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AccountCost",
                table: "SellingPayment",
                nullable: false,
                computedColumnSql: "([PaidAmount] * ([AccountCostPercentage]/100))");

            migrationBuilder.AddColumn<decimal>(
                name: "SellingProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost]))");

            migrationBuilder.AddColumn<double>(
                name: "ServiceProfit",
                table: "Selling",
                nullable: false,
                computedColumnSql: "([ServiceCharge]-[ServiceCost])");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(maxLength: 128, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CostPercentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "AccountDeposit",
                columns: table => new
                {
                    AccountDepositId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    DepositDateUtc = table.Column<DateTime>(type: "date", nullable: false),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDeposit", x => x.AccountDepositId);
                    table.ForeignKey(
                        name: "FK_AccountDeposit_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "AccountWithdraw",
                columns: table => new
                {
                    AccountWithdrawId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    WithdrawAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    WithdrawDateUtc = table.Column<DateTime>(type: "date", nullable: false),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountWithdraw", x => x.AccountWithdrawId);
                    table.ForeignKey(
                        name: "FK_AccountWithdraw_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellingPayment_AccountId",
                table: "SellingPayment",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDeposit_AccountId",
                table: "AccountDeposit",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountWithdraw_AccountId",
                table: "AccountWithdraw",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellingPayment_Account",
                table: "SellingPayment",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellingPayment_Account",
                table: "SellingPayment");

            migrationBuilder.DropTable(
                name: "AccountDeposit");

            migrationBuilder.DropTable(
                name: "AccountWithdraw");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropIndex(
                name: "IX_SellingPayment_AccountId",
                table: "SellingPayment");

            migrationBuilder.DropColumn(
                name: "AccountCost",
                table: "SellingPayment");

            migrationBuilder.DropColumn(
                name: "AccountCostPercentage",
                table: "SellingPayment");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "SellingPayment");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "SellingList");

            migrationBuilder.DropColumn(
                name: "BuyingTotalPrice",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "Expense",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "ExpenseDescription",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "SellingAccountCost",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "SellingProfit",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "ServiceCharge",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "ServiceChargeDescription",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "ServiceCost",
                table: "Selling");

            migrationBuilder.DropColumn(
                name: "ServiceProfit",
                table: "Selling");
        }
    }
}
