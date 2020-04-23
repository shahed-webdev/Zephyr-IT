using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ServicePaymentList
    {
        public int ServicePaymentListId { get; set; }
        public int SellingPaymentId { get; set; }
        public int ServiceId { get; set; }
        public double ServicePaidAmount { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual SellingPayment SellingPayment { get; set; }
        public virtual Service Service { get; set; }
    }
}
