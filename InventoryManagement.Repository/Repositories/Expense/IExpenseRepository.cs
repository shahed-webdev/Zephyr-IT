using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IExpenseRepository : IRepository<Expense>, IAddCustom<ExpenseViewModel>
    {
        ICollection<ExpenseViewModel> ToListCustom();
        Task<ICollection<ExpenseViewModel>> ToListCustomAsync();
        // void AddCustom(ExpenseViewModel model);
        void RemoveCustom(int id);
        ICollection<int> Years();
        double ExpenseYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);

        ICollection<ExpenseViewModel> DateToDate(DateTime? sDateTime, DateTime? eDateTime);

        ICollection<ExpenseCategoryWise> CategoryWistDateToDate(DateTime? sDateTime, DateTime? eDateTime);


    }
}
