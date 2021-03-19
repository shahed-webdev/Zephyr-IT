using System;

namespace InventoryManagement.Repository
{
    public class PurchasePaymentViewModel
    {

    }

    public class PurchasePaymentListViewModel
    {
        public string PaymentMethod { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }
    }

    public class PurchaseDuePaySingleModel
    {
        public int PurchaseId { get; set; }
        public int VendorId { get; set; }
        public int RegistrationId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }

        public int? AccountId { get; set; }
    }
}