using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Purchase
    {
        public Purchase()
        {
            Product = new HashSet<Product>();
            PurchaseAdjustment = new HashSet<PurchaseAdjustment>();
            PurchasePaymentList = new HashSet<PurchasePaymentList>();
        }

        public int PurchaseId { get; set; }
        public int RegistrationId { get; set; }
        public int VendorId { get; set; }
        public int PurchaseSn { get; set; }
        public double PurchaseTotalPrice { get; set; }
        public double PurchaseDiscountAmount { get; set; }
        public double PurchaseDiscountPercentage { get; set; }
        public double PurchasePaidAmount { get; set; }
        public double PurchaseReturnAmount { get; set; }
        public double PurchaseDueAmount { get; set; }
        public string PurchasePaymentStatus { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual Registration Registration { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<PurchaseAdjustment> PurchaseAdjustment { get; set; }
        public virtual ICollection<PurchasePaymentList> PurchasePaymentList { get; set; }
    }
}
