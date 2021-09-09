using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class ExpenseTransportationAddModel
    {
        public int? CustomerId { get; set; }
        public int? AccountId { get; set; }
        public decimal TotalExpense { get; set; }
        public string ExpenseNote { get; set; }
        public DateTime ExpenseDate { get; set; }
        public List<ExpenseTransportationListAddModel> ExpenseTransportationList { get; set; }
    }

    public class ExpenseTransportationListAddModel
    {
        public int ExpenseTransportationListId { get; set; }
        public int NumberOfPerson { get; set; }
        public string ExpenseFor { get; set; }
        public string Vehicle { get; set; }
        public decimal ExpenseAmount { get; set; }
    }

    public class ExpenseTransportationDetailsModel
    {
        public int ExpenseTransportationId { get; set; }
        public int? CustomerId { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public bool IsApproved { get; set; }
        public string CustomerName { get; set; }
        public string CreateBy { get; set; }
        public int VoucherNo { get; set; }
        public decimal TotalExpense { get; set; }
        public string ExpenseNote { get; set; }
        public DateTime ExpenseDate { get; set; }
        public List<ExpenseTransportationListAddModel> ExpenseTransportationList { get; set; }
    }
}