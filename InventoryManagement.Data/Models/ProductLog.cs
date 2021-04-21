namespace InventoryManagement.Data
{
    public class ProductLog
    {
        public int ProductLogId { get; set; }
        public ProductStock ProductStock { get; set; }
        public int ProductStockId { get; set; }
    }
}