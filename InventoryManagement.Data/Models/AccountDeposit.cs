using System;

namespace InventoryManagement.Data
{
    public class AccountDeposit
    {
        public int AccountDepositId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public decimal DepositAmount { get; set; }
        public string Description { get; set; }
        public DateTime WDepositDate { get; set; }
        public DateTime InsertDate { get; set; }


    }
}