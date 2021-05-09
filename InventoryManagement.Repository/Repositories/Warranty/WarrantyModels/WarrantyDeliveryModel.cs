namespace InventoryManagement.Repository
{
    public class WarrantyDeliveryModel
    {
        public int SellingId { get; set; }
        public int WarrantyId { get; set; }
        public int ProductStockId { get; set; }
        public int? ChangedProductCatalogId { get; set; }
        public string DeliveryDescription { get; set; }
        public string ChangedProductName { get; set; }
        public string ChangedProductCode { get; set; }
    }
}