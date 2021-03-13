using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public class PurchaseList
    {
        public PurchaseList()
        {
            ProductStock = new HashSet<ProductStock>();
        }
        public int PurchaseListId { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductStock> ProductStock { get; set; }
    }
}