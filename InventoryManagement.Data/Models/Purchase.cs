using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Purchase
    {
        public Purchase()
        {
            PurchaseList = new HashSet<PurchaseList>();
            PurchasePaymentList = new HashSet<PurchasePaymentList>();
            ProductLog = new HashSet<ProductLog>();
            PurchasePaymentReturnRecord = new HashSet<PurchasePaymentReturnRecord>();
        }

        public int PurchaseId { get; set; }
        public int RegistrationId { get; set; }
        public int VendorId { get; set; }
        public int PurchaseSn { get; set; }
        public decimal PurchaseTotalPrice { get; set; }
        public decimal PurchaseDiscountAmount { get; set; }
        public decimal PurchaseDiscountPercentage { get; set; }
        public decimal PurchasePaidAmount { get; set; }
        public decimal PurchaseReturnAmount { get; set; }
        public decimal PurchaseDueAmount { get; set; }
        public string PurchasePaymentStatus { get; set; }
        public string MemoNumber { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual Registration Registration { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Selling Selling { get; set; }
        public virtual ICollection<PurchaseList> PurchaseList { get; set; }
        public virtual ICollection<PurchasePaymentList> PurchasePaymentList { get; set; }
        public virtual ICollection<ProductLog> ProductLog { get; set; }
        public virtual ICollection<PurchasePaymentReturnRecord> PurchasePaymentReturnRecord { get; set; }
    }
}
