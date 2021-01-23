namespace InventoryManagement.Repository
{
    public class ExpenseFixedAddModel
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public int IntervalDays { get; set; }
    }

    public class ExpenseFixedViewModel
    {
        public int ExpenseFixedId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int IntervalDays { get; set; }
        public double CostPerDay { get; set; }
    }
}