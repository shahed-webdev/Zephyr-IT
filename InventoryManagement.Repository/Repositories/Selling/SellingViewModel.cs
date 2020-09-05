using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class SellingViewModel
    {
        public SellingViewModel()
        {
            ProductList = new HashSet<SellingProductListViewModel>();
        }
        public int SellingId { get; set; }
        public int RegistrationId { get; set; }
        public int CustomerId { get; set; }
        public double SellingTotalPrice { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime SellingDate { get; set; }
        //public string[] ProductCodes { get; set; }
        public ICollection<SellingProductListViewModel> ProductList { get; set; }
    }

    public class SellingProductListViewModel
    {
        public int ProductId { get; set; }
        public double SellingPrice { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string[] ProductCodes { get; set; }
    }

    public class SellingReceiptProductViewModel
    {

        public int SellingListId { get; set; }
        public int SellingId { get; set; }
        public int ProductId { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public double SellingPrice { get; set; }
        public double PurchasePrice { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Warranty { get; set; }
        public string[] ProductCodes { get; set; }
    }

    public class SellingReceiptViewModel
    {
        public SellingReceiptViewModel()
        {
            this.Products = new HashSet<SellingReceiptProductViewModel>();
            this.Payments = new HashSet<SellingPaymentViewModel>();
        }
        public InstitutionVM InstitutionInfo { get; set; }
        public int SellingSn { get; set; }
        public int SellingId { get; set; }
        public double SellingTotalPrice { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingDueAmount { get; set; }
        public double SellingReturnAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public ICollection<SellingReceiptProductViewModel> Products { get; set; }
        public ICollection<SellingPaymentViewModel> Payments { get; set; }
        public CustomerReceiptViewModel CustomerInfo { get; set; }
        public string SoldBy { get; set; }
    }



    public class SellingRecordViewModel
    {
        public int SellingId { get; set; }
        public int CustomerId { get; set; }
        public int SellingSn { get; set; }
        public string CustomerName { get; set; }
        public double SellingAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingDueAmount { get; set; }
        public DateTime SellingDate { get; set; }
    }

    public class SellingUpdateGetModel
    {
        public SellingUpdateGetModel()
        {
        }
        public CustomerReceiptViewModel CustomerInfo { get; set; }
        public int SellingSn { get; set; }
        public int SellingId { get; set; }
        public double SellingTotalPrice { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingDueAmount { get; set; }
        public double SellingReturnAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public string SoildBy { get; set; }

        public ICollection<SellingReceiptProductViewModel> Products { get; set; }
        public ICollection<SellingPaymentViewModel> Payments { get; set; }
    }

    public class SellingUpdatePostModel
    {
        public int SellingId { get; set; }
        public int UpdateRegistrationId { get; set; }
        public double SellingTotalPrice { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingReturnAmount { get; set; }
        public string[] AddedProductCodes { get; set; }
        public string[] RemovedProductCodes { get; set; }
        public double PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaidDate { get; set; }
        public ICollection<SellingUpdateProductPostModel> Products { get; set; }
    }

    public class SellingUpdateProductPostModel
    {
        public int SellingListId { get; set; }
        public int ProductId { get; set; }
        public double SellingPrice { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
    }
}