using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public class Account
    {
        public Account()
        {
            AccountDeposit = new HashSet<AccountDeposit>();
            AccountWithdraw = new HashSet<AccountWithdraw>();
            SellingPayment = new HashSet<SellingPayment>();
            PurchasePayment = new HashSet<PurchasePayment>();
            Expense = new HashSet<Expense>();
            ExpenseTransportation = new HashSet<ExpenseTransportation>();
            SellingPaymentReturnRecord = new HashSet<SellingPaymentReturnRecord>();
            PurchasePaymentReturnRecord = new HashSet<PurchasePaymentReturnRecord>();
            SellingExpense = new HashSet<SellingExpense>();

        }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal CostPercentage { get; set; }
        public Institution Institution { get; set; }
        public virtual ICollection<AccountWithdraw> AccountWithdraw { get; set; }
        public virtual ICollection<AccountDeposit> AccountDeposit { get; set; }
        public virtual ICollection<SellingPayment> SellingPayment { get; set; }
        public virtual ICollection<PurchasePayment> PurchasePayment { get; set; }
        public virtual ICollection<Expense> Expense { get; set; }
        public virtual ICollection<ExpenseTransportation> ExpenseTransportation { get; set; }
        public virtual ICollection<SellingPaymentReturnRecord> SellingPaymentReturnRecord { get; set; }
        public virtual ICollection<SellingExpense> SellingExpense { get; set; }
        public virtual ICollection<PurchasePaymentReturnRecord> PurchasePaymentReturnRecord { get; set; }
    }
}