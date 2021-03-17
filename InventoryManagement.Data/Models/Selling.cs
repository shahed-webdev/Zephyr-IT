using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public class Selling
    {
        public Selling()
        {
            SellingAdjustment = new HashSet<SellingAdjustment>();
            SellingList = new HashSet<SellingList>();
            SellingPaymentList = new HashSet<SellingPaymentList>();
            SellingExpense = new HashSet<SellingExpense>();
        }

        public int SellingId { get; set; }
        public int RegistrationId { get; set; }
        public int CustomerId { get; set; }
        public int SellingSn { get; set; }
        public decimal SellingTotalPrice { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public decimal SellingDiscountPercentage { get; set; }
        public decimal SellingPaidAmount { get; set; }
        public decimal SellingReturnAmount { get; set; }
        public decimal SellingDueAmount { get; set; }
        public string SellingPaymentStatus { get; set; }
        public DateTime SellingDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime InsertDate { get; set; }

        public DateTime? PromisedPaymentDate { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal ServiceCost { get; set; }
        //[ServiceCharge]-[ServiceCost]
        public decimal ServiceProfit { get; set; }
        public string ServiceChargeDescription { get; set; }
        public decimal ExpenseTotal { get; set; }
        public decimal BuyingTotalPrice { get; set; }
        public decimal SellingAccountCost { get; set; }
        //[BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])
        public decimal SellingProfit { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Registration Registration { get; set; }
        public virtual ICollection<SellingAdjustment> SellingAdjustment { get; set; }
        public virtual ICollection<SellingList> SellingList { get; set; }
        public virtual ICollection<SellingPaymentList> SellingPaymentList { get; set; }
        public virtual ICollection<SellingExpense> SellingExpense { get; set; }

    }
}
