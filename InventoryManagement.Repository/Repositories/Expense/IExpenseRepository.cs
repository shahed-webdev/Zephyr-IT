﻿using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IExpenseRepository : IRepository<Expense>, IAddCustom<ExpenseVM>
    {
        ICollection<ExpenseVM> ToListCustom();
        Task<ICollection<ExpenseVM>> ToListCustomAsync();
        // void AddCustom(ExpenseVM model);
        void RemoveCustom(int id);
        ICollection<int> Years();
        double ExpenseYearly(int year);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);

        ICollection<ExpenseVM> DateToDate(DateTime? sDateTime, DateTime? eDateTime);

        ICollection<ExpenseCategoryWise> CategoryWistDateToDate(DateTime? sDateTime, DateTime? eDateTime);


    }
}