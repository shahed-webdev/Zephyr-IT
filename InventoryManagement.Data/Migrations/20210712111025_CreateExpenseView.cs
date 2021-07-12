using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class CreateExpenseView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create view VW_ExpenseWithTransportation 
as 
SELECT Expense.ExpenseId AS Id, Expense.VoucherNo, Expense.IsApproved, CAST(0 AS bit) AS IsTransportation, Registration.UserName AS CreateBy, ExpenseCategory.CategoryName AS ExpenseCategory, Expense.ExpenseAmount, Expense.ExpenseFor, Expense.ExpenseDate
FROM  Expense INNER JOIN
Registration ON Expense.RegistrationId = Registration.RegistrationId INNER JOIN
ExpenseCategory ON Expense.ExpenseCategoryId = ExpenseCategory.ExpenseCategoryId
UNION ALL
SELECT ExpenseTransportation.ExpenseTransportationId AS Id, ExpenseTransportation.VoucherNo, ExpenseTransportation.IsApproved, CAST(0 AS bit) AS IsTransportation, Registration.UserName AS CreateBy, 'Transportation' AS ExpenseCategory, ExpenseTransportation.TotalExpense, ExpenseTransportation.ExpenseNote, ExpenseTransportation.ExpenseDate
FROM ExpenseTransportation INNER JOIN
Registration ON ExpenseTransportation.RegistrationId = Registration.RegistrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" drop view VW_ExpenseWithTransportation;");
        }
    }
}
