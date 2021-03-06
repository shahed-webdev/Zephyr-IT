namespace InventoryManagement.Repository 
{
    public class AccountCrudModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal CostPercentage { get; set; }
    }
}