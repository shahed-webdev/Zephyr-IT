namespace InventoryManagement.Repository
{
    public class ProductStockViewModel
    {
        public string ProductCode { get; set; }
    }

    public class ProductStockDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public double SellingPrice { get; set; }
        public double PurchasePrice { get; set; }
        public int? SellingSn { get; set; }
        public int? SellingId { get; set; }
        public int? PurchaseId { get; set; }
    }
}