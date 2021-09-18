using System;

namespace InventoryManagement.Data
{
    public class SellingExpense
    {
        public int SellingExpenseId { get; set; }
        public int SellingId { get; set; }
        public Selling Selling { get; set; }
        public decimal Expense { get; set; }
        public string ExpenseDescription { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public int? AccountId { get; set; }
        public Account Account { get; set; }
    }
}