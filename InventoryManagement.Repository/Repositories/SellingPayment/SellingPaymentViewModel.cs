using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class SellingPaymentViewModel
    {
        public string PaymentMethod { get; set; }
        public double PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }
    }

    public class SellingDuePaySingleModel
    {
        public int SellingId { get; set; }
        public int CustomerId { get; set; }
        public int RegistrationId { get; set; }
        public string PaymentMethod { get; set; }
        public double PaidAmount { get; set; }
        public double SellingDiscountAmount { get; set; }
        public DateTime PaidDate { get; set; }
    }

    public class SellingDuePayMultipleModel
    {
        public SellingDuePayMultipleModel()
        {
            Bills = new HashSet<SellingDuePayMultipleBill>();
        }
        public int CustomerId { get; set; }
        public int RegistrationId { get; set; }
        public double PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaidDate { get; set; }
        public ICollection<SellingDuePayMultipleBill> Bills { get; set; }
    }

    public class SellingDuePayMultipleBill
    {
        public int SellingId { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingDiscountAmount { get; set; }

    }

    public class SellingPaymentRecordModel
    {
        public int SellingPaymentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ReceiptSn { get; set; }
        public double PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaidDate { get; set; }
    }
}