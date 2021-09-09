using System;

namespace InventoryManagement.Data
{
    public class VW_ExpenseWithTransportation
    {
        public int Id { get; set; }
        public int VoucherNo { get; set; }
        public bool IsApproved { get; set; }
        public bool IsTransportation { get; set; }
        public string CreateBy { get; set; }
        public string ExpenseCategory { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string ExpenseFor { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
    }
}