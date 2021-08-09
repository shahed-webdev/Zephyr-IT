using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class PurchaseViewModel
    {
        public PurchaseViewModel()
        {
            this.Products = new HashSet<ProductPurchaseViewModel>();
        }

        public int PurchaseId { get; set; }
        public int RegistrationId { get; set; }
        public int VendorId { get; set; }
        public decimal PurchaseTotalPrice { get; set; }
        public decimal PurchaseDiscountAmount { get; set; }
        public decimal PurchasePaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string MemoNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<ProductPurchaseViewModel> Products { get; set; }
        public int? AccountId { get; set; }
    }

    public class PurchaseReceiptViewModel
    {
        public PurchaseReceiptViewModel()
        {
            this.Products = new HashSet<ProductViewModel>();
            this.Payments = new HashSet<PurchasePaymentListViewModel>();
        }
        public InstitutionVM InstitutionInfo { get; set; }
        public int PurchaseSn { get; set; }
        public int PurchaseId { get; set; }
        public decimal PurchaseTotalPrice { get; set; }
        public decimal PurchaseDiscountAmount { get; set; }
        public decimal PurchasePaidAmount { get; set; }
        public decimal PurchaseDueAmount { get; set; }
        public decimal PurchaseReturnAmount { get; set; }
        public string MemoNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }
        public ICollection<PurchasePaymentListViewModel> Payments { get; set; }
        public VendorViewModel VendorInfo { get; set; }
        public string SoildBy { get; set; }
    }

    public class PurchaseRecordViewModel
    {
        public int PurchaseId { get; set; }
        public int VendorId { get; set; }
        public string VendorCompanyName { get; set; }
        public int PurchaseSn { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal PurchasePaidAmount { get; set; }
        public decimal PurchaseDiscountAmount { get; set; }
        public decimal PurchaseDueAmount { get; set; }
        public string MemoNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public class PurchaseGetByReceiptModel
    {
        public decimal PurchaseAdjustedAmount { get; set; }
        public string PurchaseDescription { get; set; }
        public int? PurchaseId { get; set; }
    }


    public class PurchaseUpdateGetModel
    {
        public PurchaseUpdateGetModel()
        {
            PurchaseList = new HashSet<ProductViewModel>();
        }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCompanyName { get; set; }
        public string VendorPhone { get; set; }
        public int PurchaseId { get; set; }
        public int PurchaseSn { get; set; }
        public string MemoNumber { get; set; }
        public decimal PurchaseTotalPrice { get; set; }
        public decimal PurchaseDiscountAmount { get; set; }
        public decimal PurchaseDueAmount { get; set; }
        public decimal PurchaseReturnAmount { get; set; }
        public decimal PurchasePaidAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<ProductViewModel> PurchaseList { get; set; }
    }

    public class PurchaseUpdatePostModel
    {
        public PurchaseUpdatePostModel()
        {
            PurchaseList = new HashSet<PurchaseUpdateProductPostModel>();
            RemovedProductStockIds = new int[] { };
        }
        public int PurchaseId { get; set; }
        public decimal PurchaseTotalPrice { get; set; }
        public decimal PurchaseDiscountAmount { get; set; }
        public decimal PurchaseReturnAmount { get; set; }
        public int[] RemovedProductStockIds { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }
        public ICollection<PurchaseUpdateProductPostModel> PurchaseList { get; set; }
        public int? AccountId { get; set; }
    }

    public class PurchaseUpdateProductPostModel
    {
        public PurchaseUpdateProductPostModel()
        {
            AddedProductCodes = new string[]{ };
        }
        public int PurchaseListId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string[] AddedProductCodes { get; set; }
    }
}