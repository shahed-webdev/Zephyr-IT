using System;

namespace InventoryManagement.Data
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public int VoucherNo { get; set; }
        public bool IsApproved { get; set; }
        public int RegistrationId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string ExpenseFor { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime InsertDate { get; set; }
        public int? AccountId { get; set; }
        public Account Account { get; set; }
        public virtual ExpenseCategory ExpenseCategory { get; set; }
        public virtual Registration Registration { get; set; }
    }
}
