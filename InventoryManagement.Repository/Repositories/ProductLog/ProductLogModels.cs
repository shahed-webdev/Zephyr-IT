using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class ProductLogAddModel
    {
        public int ProductStockId { get; set; }
        public int ActivityByRegistrationId { get; set; }
        public string Details { get; set; }
        public ProductLogStatus LogStatus { get; set; }
    }
}