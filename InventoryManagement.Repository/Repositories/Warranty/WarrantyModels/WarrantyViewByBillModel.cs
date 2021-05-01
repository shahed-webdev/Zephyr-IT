using System;

namespace InventoryManagement.Repository
{
    public class WarrantyViewByBillModel
    {
        public int WarrantyId { get; set; }
        public int WarrantySn { get; set; }
        public string DeliveryDescription { get; set; }
        public string ProductCode { get; set; }
        public string ProductCatalogName { get; set; }
        public string ChangedProductCode { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsDelivered { get; set; }
    }
}