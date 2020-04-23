using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class SellingAdjustment
    {
        public int SellingAdjustmentId { get; set; }
        public int RegistrationId { get; set; }
        public int ProductId { get; set; }
        public int SellingId { get; set; }
        public int ProductStockId { get; set; }
        public string AdjustmentStatus { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductStock ProductStock { get; set; }
        public virtual Registration Registration { get; set; }
        public virtual Selling Selling { get; set; }
    }
}
