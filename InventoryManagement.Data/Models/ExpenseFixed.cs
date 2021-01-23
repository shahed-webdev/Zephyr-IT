using System;

namespace InventoryManagement.Data
{
    public class ExpenseFixed
    {
        public int ExpenseFixedId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int IntervalDays { get; set; }
        public double CostPerDay { get; set; }
        public DateTime InsertDate { get; set; }
    }
}