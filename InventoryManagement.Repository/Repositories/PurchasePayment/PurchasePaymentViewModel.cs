using System;

namespace InventoryManagement.Repository
{
    public class PurchasePaymentViewModel
    {

    }

    public class PurchasePaymentListViewModel
    {
        public string PaymentMethod { get; set; }
        public double PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }
    }
}