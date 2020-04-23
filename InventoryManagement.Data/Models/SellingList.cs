using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class SellingList
    {
        public int SellingListId { get; set; }
        public int SellingId { get; set; }
        public int ProductStockId { get; set; }
        public double SellingPrice { get; set; }

        public virtual ProductStock ProductStock { get; set; }
        public virtual Selling Selling { get; set; }
    }
}
