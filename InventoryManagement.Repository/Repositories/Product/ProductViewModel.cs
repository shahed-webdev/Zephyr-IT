using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            this.ProductStocks = new HashSet<ProductStockViewModel>();
        }
        public int ProductId { get; set; }
        public int PurchaseListId { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public ICollection<ProductStockViewModel> ProductStocks { get; set; }
    }

    public class ProductPurchaseViewModel
    {
        public ProductPurchaseViewModel()
        {
            this.ProductStocks = new HashSet<ProductStockViewModel>();
        }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public ICollection<ProductStockViewModel> ProductStocks { get; set; }
    }

    public class ProductSellViewModel
    {
        public int ProductId { get; set; }
        public int ProductStockId { get; set; }
        public string ProductCode { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
    }

    public class ProductShowViewModel
    {
        public int ProductId { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal SellingPrice { get; set; }
        public int Stock { get; set; }
    }
}