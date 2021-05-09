using System;

namespace InventoryManagement.Data
{
    public class ProductLog
    {
        public int ProductLogId { get; set; }
        public ProductStock ProductStock { get; set; }
        public int ProductStockId { get; set; }
        public string Details { get; set; }
        public ProductLogStatus LogStatus { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int ActivityByRegistrationId { get; set; }
        public Registration Registration { get; set; }
        public int? SellingId { get; set; }
        public int? PurchaseId { get; set; }
        public virtual Selling Selling { get; set; }
        public virtual Purchase Purchase { get; set; }

    }
}