using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class SellingViewModel
    {
        public int SellingId { get; set; }
        public int RegistrationId { get; set; }
        public int CustomerId { get; set; }
        public double SellingTotalPrice { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime SellingDate { get; set; }
        public string[] ProductCodes { get; set; }
    }

    public class SellingReceiptViewModel
    {
        public SellingReceiptViewModel()
        {
            this.Products = new HashSet<ProductSellViewModel>();
            this.Payments = new HashSet<SellingPaymentViewModel>();
        }
        public InstitutionVM InstitutionInfo { get; set; }
        public int SellingSn { get; set; }
        public int SellingId { get; set; }
        public double SellingTotalPrice { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingDueAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public ICollection<ProductSellViewModel> Products { get; set; }
        public ICollection<SellingPaymentViewModel> Payments { get; set; }
        public CustomerReceiptViewModel CustomerInfo { get; set; }
        public string SoildBy { get; set; }
    }

    public class SellingRecordViewModel
    {

    }
}