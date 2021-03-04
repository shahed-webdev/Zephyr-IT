using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class SellingList
    {
        public SellingList()
        {
            ProductStock = new HashSet<ProductStock>();
        }

        public int SellingListId { get; set; }
        public int SellingId { get; set; }
        public int ProductId { get; set; }
        public double SellingPrice { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public virtual Selling Selling { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductStock> ProductStock { get; set; }
        
        public double PurchasePrice { get; set; }
    }
}
