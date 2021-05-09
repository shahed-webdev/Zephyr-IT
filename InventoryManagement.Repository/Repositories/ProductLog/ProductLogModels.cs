using InventoryManagement.Data;
using System;

namespace InventoryManagement.Repository
{
    public class ProductLogAddModel
    {
        public int ProductStockId { get; set; }
        public int ActivityByRegistrationId { get; set; }
        public string Details { get; set; }
        public ProductLogStatus LogStatus { get; set; }
        public int? SellingId { get; set; }
        public int? PurchaseId { get; set; }
    }

    public class ProductLogViewModel
    {
        public int ActivityByRegistrationId { get; set; }
        public string ActivityBy { get; set; }
        public string Details { get; set; }
        public string LogStatus { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int? SellingId { get; set; }
        public int SellingSn { get; set; }
    }
}