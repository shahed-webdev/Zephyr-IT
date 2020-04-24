using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Expense
    {
        public int ExpenseId { get; set; }
        public int RegistrationId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public double ExpenseAmount { get; set; }
        public string ExpenseFor { get; set; }
        public string ExpensePaymentMethod { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual ExpenseCategory ExpenseCategory { get; set; }
        public virtual Registration Registration { get; set; }
    }
}
