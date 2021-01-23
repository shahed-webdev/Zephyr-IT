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
        Task<List<ExpenseViewModel>> ToListCustomAsync();
        void AddCustom(ExpenseAddModel model, int voucherNo, bool isApproved);
        void RemoveCustom(int id);
        ICollection<int> Years();
        double DailyExpenseAmount(DateTime? day);
        double ExpenseYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);

        ICollection<ExpenseViewModel> DateToDate(DateTime? sDateTime, DateTime? eDateTime);

        ICollection<ExpenseCategoryWise> CategoryWistDateToDate(DateTime? sDateTime, DateTime? eDateTime);


    }
}
