using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public class Registration
    {
        public Registration()
        {
            Expense = new HashSet<Expense>();
            PageLinkAssign = new HashSet<PageLinkAssign>();
            Purchase = new HashSet<Purchase>();
            PurchasePayment = new HashSet<PurchasePayment>();
            Selling = new HashSet<Selling>();
            SellingAdjustment = new HashSet<SellingAdjustment>();
            SellingPayment = new HashSet<SellingPayment>();
            Service = new HashSet<Service>();
            ExpenseTransportation = new HashSet<ExpenseTransportation>();
            ProductLog = new HashSet<ProductLog>();
            AdminMoneyCollection = new HashSet<AdminMoneyCollection>();
            SellingPromiseDateMisses = new List<SellingPromiseDateMiss>();
            SellingPaymentReturnRecord = new HashSet<SellingPaymentReturnRecord>();
            PurchasePaymentReturnRecord = new HashSet<PurchasePaymentReturnRecord>();
        }

        public int RegistrationId { get; set; }
        public string UserName { get; set; }
        public bool? Validation { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Designation { get; set; }
        public string DateofBirth { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreateDate { get; set; }
        public string Ps { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }
        public virtual ICollection<PageLinkAssign> PageLinkAssign { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
        public virtual ICollection<PurchasePayment> PurchasePayment { get; set; }
        public virtual ICollection<Selling> Selling { get; set; }
        public virtual ICollection<SellingAdjustment> SellingAdjustment { get; set; }
        public virtual ICollection<SellingPayment> SellingPayment { get; set; }
        public virtual ICollection<Service> Service { get; set; }
        public virtual ICollection<ExpenseTransportation> ExpenseTransportation { get; set; }
        public virtual ICollection<ProductLog> ProductLog { get; set; }
        public virtual ICollection<AdminMoneyCollection> AdminMoneyCollection { get; set; }
        public virtual ICollection<SellingPromiseDateMiss> SellingPromiseDateMisses { get; set; }
        public virtual ICollection<SellingPaymentReturnRecord> SellingPaymentReturnRecord { get; set; }
        public virtual ICollection<PurchasePaymentReturnRecord> PurchasePaymentReturnRecord { get; set; }

    }
}
