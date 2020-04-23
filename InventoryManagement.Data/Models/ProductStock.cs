using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ProductStock
    {
        public ProductStock()
        {
            SellingAdjustment = new HashSet<SellingAdjustment>();
            SellingList = new HashSet<SellingList>();
        }

        public int ProductStockId { get; set; }
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public bool IsSold { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<SellingAdjustment> SellingAdjustment { get; set; }
        public virtual ICollection<SellingList> SellingList { get; set; }
    }
}
