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
        public decimal SellingTotalPrice { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public decimal SellingPaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime SellingDate { get; set; }
        //public string[] ProductCodes { get; set; }
        public ICollection<SellingProductListViewModel> ProductList { get; set; }

        public DateTime? PromisedPaymentDate { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal ServiceCost { get; set; }
        public string ServiceChargeDescription { get; set; }
        public decimal Expense { get; set; }
        public string ExpenseDescription { get; set; }
        public int? AccountId { get; set; }
    }

    public class SellingProductListViewModel
    {
        public int ProductId { get; set; }
        public decimal SellingPrice { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string[] ProductCodes { get; set; }

        public decimal PurchasePrice { get; set; }
    }

    public class SellingReceiptProductViewModel
    {
        public int SellingListId { get; set; }
        public int SellingId { get; set; }
        public int ProductId { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
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
            this.SellingExpenses = new HashSet<SellingExpenseListModel>();
        }
        public InstitutionVM InstitutionInfo { get; set; }
        public int SellingSn { get; set; }
        public int SellingId { get; set; }
        public decimal SellingTotalPrice { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public decimal SellingPaidAmount { get; set; }
        public decimal SellingDueAmount { get; set; }
        public decimal SellingReturnAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public ICollection<SellingReceiptProductViewModel> Products { get; set; }
        public ICollection<SellingPaymentViewModel> Payments { get; set; }
        public ICollection<SellingExpenseListModel> SellingExpenses { get; set; }
        public CustomerReceiptViewModel CustomerInfo { get; set; }
        public string SoldBy { get; set; }
        public decimal ServiceCharge { get; set; }
        public string ServiceChargeDescription { get; set; }
        public DateTime? PromisedPaymentDate { get; set; }
    }



    public class SellingRecordViewModel
    {
        public int SellingId { get; set; }
        public int CustomerId { get; set; }
        public int SellingSn { get; set; }
        public string CustomerName { get; set; }
        public decimal SellingAmount { get; set; }
        public decimal SellingPaidAmount { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public decimal SellingDueAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime? PromisedPaymentDate { get; set; }
    }

    public class SellingUpdateGetModel
    {
        public SellingUpdateGetModel()
        {
            this.Products = new HashSet<SellingReceiptProductViewModel>();
            this.Payments = new HashSet<SellingPaymentViewModel>();
            this.SellingExpenses = new HashSet<SellingExpenseListModel>();
        }
        public CustomerReceiptViewModel CustomerInfo { get; set; }
        public int SellingSn { get; set; }
        public int SellingId { get; set; }
        public decimal SellingTotalPrice { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public decimal SellingDueAmount { get; set; }
        public decimal SellingReturnAmount { get; set; }
        public decimal SellingPaidAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public string SoildBy { get; set; }

        public ICollection<SellingReceiptProductViewModel> Products { get; set; }
        public ICollection<SellingPaymentViewModel> Payments { get; set; }

        public ICollection<SellingExpenseListModel> SellingExpenses { get; set; }
        public decimal ServiceCharge { get; set; }
        public string ServiceChargeDescription { get; set; }
        public DateTime? PromisedPaymentDate { get; set; }
        public decimal ServiceCost { get; set; }
        public decimal ExpenseTotal { get; set; }
    }

    public class SellingUpdatePostModel
    {
        public int SellingId { get; set; }
        public int UpdateRegistrationId { get; set; }
        public decimal SellingTotalPrice { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public decimal SellingReturnAmount { get; set; }
        public string[] AddedProductCodes { get; set; }
        public string[] RemovedProductCodes { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }
        public ICollection<SellingUpdateProductPostModel> Products { get; set; }
        public DateTime? PromisedPaymentDate { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal ServiceCost { get; set; }
        public string ServiceChargeDescription { get; set; }
        public int? AccountId { get; set; }
    }

    public class SellingUpdateProductPostModel
    {
        public int SellingListId { get; set; }
        public int ProductId { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string[] RemainCodes { get; set; }
    }

    public class DateWiseSaleSummary
    {
        public decimal SoldAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DueAmount { get; set; }
    }
    public class SellingExpenseAddModel
    {
        public int SellingId { get; set; }
        public decimal Expense { get; set; }
        public string ExpenseDescription { get; set; }
    }
    public class SellingExpenseListModel
    {
        public int SellingExpenseId { get; set; }
        public decimal Expense { get; set; }
        public string ExpenseDescription { get; set; }
        public DateTime InsertDateUtc { get; set; }
    }
}