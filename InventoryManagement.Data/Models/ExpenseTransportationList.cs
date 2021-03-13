namespace InventoryManagement.Data
{
    public class ExpenseTransportationList
    {
        public int ExpenseTransportationListId { get; set; }
        public int ExpenseTransportationId { get; set; }
        public int NumberOfPerson { get; set; }
        public string ExpenseFor { get; set; }
        public string Vehicle { get; set; }
        public decimal ExpenseAmount { get; set; }
        public virtual ExpenseTransportation ExpenseTransportation { get; set; }

    }
}