using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ExpenseCategory
    {
        public ExpenseCategory()
        {
            Expense = new HashSet<Expense>();
        }

        public int ExpenseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public double TotalExpense { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }
    }
}
