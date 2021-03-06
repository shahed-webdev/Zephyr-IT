using System;

namespace InventoryManagement.Repository 
{
    public class AccountWithdrawCrudModel
    {
        public int AccountWithdrawId { get; set; }
        public int AccountId { get; set; }
        public decimal WithdrawAmount { get; set; }
        public string Description { get; set; }
        public DateTime WithdrawDateUtc { get; set; }

    }
}