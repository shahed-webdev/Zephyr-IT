using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class SellingPayment
    {
        public SellingPayment()
        {
            SellingPaymentList = new HashSet<SellingPaymentList>();
            ServicePaymentList = new HashSet<ServicePaymentList>();
        }

        public int SellingPaymentId { get; set; }
        public int RegistrationId { get; set; }
        public int CustomerId { get; set; }
        public int ReceiptSn { get; set; }
        public double PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Registration Registration { get; set; }
        public virtual ICollection<SellingPaymentList> SellingPaymentList { get; set; }
        public virtual ICollection<ServicePaymentList> ServicePaymentList { get; set; }
    }
}
