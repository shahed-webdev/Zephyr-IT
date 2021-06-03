using System;
using System.Collections.Generic;

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


    public class PurchaseDuePayMultipleModel
    {
        public int PurchaseId { get; set; }
        public int VendorId { get; set; }
        public int RegistrationId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }

        public int? AccountId { get; set; }
    }


    public class VendorMultipleDueCollectionViewModel
    {
        public VendorMultipleDueCollectionViewModel()
        {
            PurchaseDueRecords = new HashSet<VendorPurchaseDueViewModel>();
        }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public decimal TotalDue { get; set; }
        public ICollection<VendorPurchaseDueViewModel> PurchaseDueRecords { get; set; }
    }
    public class VendorPurchaseDueViewModel
    {
        public int PurchaseId { get; set; }
        public int PurchaseSn { get; set; }
        public decimal PurchaseTotalPrice { get; set; }
        public decimal PurchaseDiscountAmount { get; set; }
        public decimal PurchasePaidAmount { get; set; }
        public decimal PurchaseReturnAmount { get; set; }
        public decimal PurchaseDueAmount { get; set; }
        public string MemoNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}