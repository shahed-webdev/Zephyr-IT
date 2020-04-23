using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class PurchaseAdjustment
    {
        public int PurchaseAdjustmentId { get; set; }
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public string ProductCode { get; set; }
        public string AdjustmentStatus { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}
