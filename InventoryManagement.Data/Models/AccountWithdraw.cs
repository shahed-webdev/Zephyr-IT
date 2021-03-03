using System;

namespace InventoryManagement.Data
{
    public class AccountWithdraw
    {

        public int AccountWithdrawId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public decimal WithdrawAmount { get; set; }
        public string Description { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime InsertDate { get; set; }
    }
}