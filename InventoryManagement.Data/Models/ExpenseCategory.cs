using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Data
{
    public partial class ExpenseCategory
    {
        public ExpenseCategory()
        {
            Expense = new HashSet<Expense>();
        }

        public int ExpenseCategoryId { get; set; }
        
        [Required (ErrorMessage = "Input Category Name!")]
        public string CategoryName { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }
    }
}
