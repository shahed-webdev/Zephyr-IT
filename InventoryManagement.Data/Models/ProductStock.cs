using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ProductStock
    {
        public ProductStock()
        {
            SellingAdjustment = new HashSet<SellingAdjustment>();
            ProductLog = new HashSet<ProductLog>();
            Warranty = new HashSet<Warranty>();
        }

        public int ProductStockId { get; set; }
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public int? SellingListId { get; set; }
        public int PurchaseListId { get; set; }
        public bool IsSold { get; set; }
        public virtual Product Product { get; set; }
        public virtual SellingList SellingList { get; set; }
        public virtual PurchaseList PurchaseList { get; set; }
        public virtual ProductDamaged ProductDamaged { get; set; }
        public virtual ICollection<SellingAdjustment> SellingAdjustment { get; set; }
        public virtual ICollection<ProductLog> ProductLog { get; set; }
        public virtual ICollection<Warranty> Warranty { get; set; }
    }
}
