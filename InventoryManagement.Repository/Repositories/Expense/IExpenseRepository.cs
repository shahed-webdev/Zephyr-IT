using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        ICollection<ExpenseViewModel> ToListCustom();
        DataResult<ExpenseAllViewModel> RecordsDataTable(DataRequest request);
        List<ExpenseAllViewModel> Records();
        Task<List<ExpenseViewModel>> ToListCustomAsync();
        void AddCustom(ExpenseAddModel model, int registrationId, int voucherNo, bool isApproved);
        decimal Approved(int expenseId, int? accountId);
        ExpenseUpdateAccountUpdateModel Edit(ExpenseAddModel model);
        ExpenseDetailsModel GetDetails(int expenseId);
        void RemoveCustom(int id);
        ICollection<int> Years();
        decimal DailyExpenseAmount(DateTime? day);
        decimal FixedExpensePerDay();
        decimal ExpenseYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);

        ICollection<ExpenseViewModel> DateToDate(DateTime? sDateTime, DateTime? eDateTime);

        ICollection<ExpenseCategoryWise> CategoryWistSummaryDateToDate(DateTime? sDateTime, DateTime? eDateTime);
        ICollection<ExpenseAllViewModel> CategoryWistDetailsDateToDate(string category, DateTime? sDateTime, DateTime? eDateTime);


    }
}
