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
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public double PurchasePrice { get; set; }
        public double SellingPrice { get; set; }
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
        public double SellingPrice { get; set; }
        public double PurchasePrice { get; set; }
        public ICollection<ProductStockViewModel> ProductStocks { get; set; }
    }

    public class ProductSellViewModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public double SellingPrice { get; set; }
    }

    public class ProductShowViewModel
    {
        public int ProductId { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public double SellingPrice { get; set; }
        public int Stock { get; set; }

    }
}