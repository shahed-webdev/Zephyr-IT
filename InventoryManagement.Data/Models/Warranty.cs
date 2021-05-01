using System;

namespace InventoryManagement.Data
{
    public class Warranty
    {
        public int WarrantyId { get; set; }
        public int SellingId { get; set; }
        public int ProductStockId { get; set; }
        public int? ChangedProductCatalogId { get; set; }
        public int WarrantySn { get; set; }
        public string AcceptanceDescription { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public string DeliveryDescription { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ChangedProductName { get; set; }
        public string ChangedProductCode { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public Selling Selling { get; set; }
        public ProductStock ProductStock { get; set; }
        public ProductCatalog ProductCatalog { get; set; }

    }
}