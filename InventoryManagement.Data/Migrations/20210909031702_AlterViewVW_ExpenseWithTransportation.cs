using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AlterViewVW_ExpenseWithTransportation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Alter view VW_ExpenseWithTransportation 
                                 as 
             SELECT Expense.ExpenseId AS Id, Expense.VoucherNo, Expense.IsApproved, CAST(0 AS bit) AS IsTransportation, Registration.UserName AS CreateBy, ExpenseCategory.CategoryName AS ExpenseCategory, Expense.ExpenseAmount, 
                         Expense.ExpenseFor, Expense.ExpenseDate, Expense.AccountId, Account.AccountName
             FROM Expense INNER JOIN
                         Registration ON Expense.RegistrationId = Registration.RegistrationId INNER JOIN
                         ExpenseCategory ON Expense.ExpenseCategoryId = ExpenseCategory.ExpenseCategoryId LEFT OUTER JOIN
                         Account ON Expense.AccountId = Account.AccountId
             UNION ALL
             SELECT ExpenseTransportation.ExpenseTransportationId AS Id, ExpenseTransportation.VoucherNo, ExpenseTransportation.IsApproved, CAST(1 AS bit) AS IsTransportation, Registration.UserName AS CreateBy, 
                         'Transportation' AS ExpenseCategory, ExpenseTransportation.TotalExpense, ExpenseTransportation.ExpenseNote, ExpenseTransportation.ExpenseDate, ExpenseTransportation.AccountId, Account.AccountName
             FROM ExpenseTransportation INNER JOIN
                         Registration ON ExpenseTransportation.RegistrationId = Registration.RegistrationId LEFT OUTER JOIN
                         Account ON ExpenseTransportation.AccountId = Account.AccountId
             ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Alter view VW_ExpenseWithTransportation 
                                 as 
             SELECT Expense.ExpenseId AS Id, Expense.VoucherNo, Expense.IsApproved, CAST(0 AS bit) AS IsTransportation, Registration.UserName AS CreateBy, ExpenseCategory.CategoryName AS ExpenseCategory, Expense.ExpenseAmount, Expense.ExpenseFor, Expense.ExpenseDate
             FROM Expense INNER JOIN
                         Registration ON Expense.RegistrationId = Registration.RegistrationId INNER JOIN
                         ExpenseCategory ON Expense.ExpenseCategoryId = ExpenseCategory.ExpenseCategoryId
             UNION ALL
             SELECT ExpenseTransportation.ExpenseTransportationId AS Id, ExpenseTransportation.VoucherNo, ExpenseTransportation.IsApproved, CAST(1 AS bit) AS IsTransportation, Registration.UserName AS CreateBy, 'Transportation' AS ExpenseCategory, ExpenseTransportation.TotalExpense, ExpenseTransportation.ExpenseNote, ExpenseTransportation.ExpenseDate
             FROM ExpenseTransportation INNER JOIN
                         Registration ON ExpenseTransportation.RegistrationId = Registration.RegistrationId
             ");
        }
    }
}
