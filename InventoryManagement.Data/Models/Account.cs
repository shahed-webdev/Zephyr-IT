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
        }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal CostPercentage { get; set; }
        public virtual ICollection<AccountWithdraw> AccountWithdraw { get; set; }
        public virtual ICollection<AccountDeposit> AccountDeposit { get; set; }
        public virtual ICollection<SellingPayment> SellingPayment { get; set; }
        public virtual ICollection<PurchasePayment> PurchasePayment { get; set; }
    }
}