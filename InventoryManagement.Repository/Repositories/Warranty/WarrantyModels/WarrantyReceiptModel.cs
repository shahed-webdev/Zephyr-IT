using System;

namespace InventoryManagement.Repository
{
    public class WarrantyReceiptModel
    {
        public int WarrantyId { get; set; }
        public int SellingId { get; set; }
        public DateTime SellingDate { get; set; }
        public int ProductStockId { get; set; }
        public int SellingSn { get; set; }
        public int WarrantySn { get; set; }
        public string AcceptanceDescription { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string ProductDescription { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductCatalogName { get; set; }
        public string DeliveryDescription { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsDelivered { get; set; }
        public string ChangedProductCatalogName { get; set; }
        public string ChangedProductName { get; set; }
        public string ChangedProductCode { get; set; }
    }
}