using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ProductStock
    {
        public ProductStock()
        {
            SellingAdjustment = new HashSet<SellingAdjustment>();
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
        public virtual ICollection<SellingAdjustment> SellingAdjustment { get; set; }
    }
}
