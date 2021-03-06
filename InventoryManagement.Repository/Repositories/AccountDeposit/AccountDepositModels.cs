using System;

namespace InventoryManagement.Repository  
{
    public class AccountDepositCrudModel
    {
        public int AccountDepositId { get; set; }
        public int AccountId { get; set; }
        public decimal DepositAmount { get; set; }
        public string Description { get; set; }
        public DateTime DepositDateUtc { get; set; }

    }
}