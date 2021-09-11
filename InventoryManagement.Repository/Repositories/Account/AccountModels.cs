namespace InventoryManagement.Repository
{
    public class AccountCrudModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal CostPercentage { get; set; }
    }

    public class SellingPaymentReturnRecordModel
    {
        public decimal PrevReturnAmount { get; set; }
        public decimal CurrentReturnAmount { get; set; }
        public int? AccountId { get; set; }
        public int SellingId { get; set; }
        public int RegistrationId { get; set; }
    }

    public class PurchasePaymentReturnRecordModel
    {
        public decimal PrevReturnAmount { get; set; }
        public decimal CurrentReturnAmount { get; set; }
        public int? AccountId { get; set; }
        public int PurchaseId { get; set; }
        public int RegistrationId { get; set; }
    }
}