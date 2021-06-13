using System;

namespace InventoryManagement.Repository
{
    public class ProductDamagedAddModel
    {
        public int ProductStockId { get; set; }
        public string ProductCode { get; set; }
        public string Note { get; set; }

    }
    public class ProductDamagedViewModel
    {
        public int ProductDamagedId { get; set; }
        public int ProductStockId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Note { get; set; }
        public decimal DamagedAmount { get; set; }
        public DateTime DamagedDate { get; set; }
    }
}