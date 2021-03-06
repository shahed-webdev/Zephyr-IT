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
        }

        public int SellingId { get; set; }
        public int RegistrationId { get; set; }
        public int CustomerId { get; set; }
        public int SellingSn { get; set; }
        public double SellingTotalPrice { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingDiscountPercentage { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingReturnAmount { get; set; }
        public double SellingDueAmount { get; set; }
        public string SellingPaymentStatus { get; set; }
        public DateTime SellingDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime InsertDate { get; set; }

        public DateTime? PromisedPaymentDate { get; set; }
        public double ServiceCharge { get; set; }
        public double ServiceCost { get; set; }
        //[ServiceCharge]-[ServiceCost]
        public double ServiceProfit { get; set; }
        public string ServiceChargeDescription { get; set; }
        public double Expense { get; set; }
        public string ExpenseDescription { get; set; }
        public double BuyingTotalPrice { get; set; }
        public decimal SellingAccountCost { get; set; }
        //[BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])
        public decimal SellingProfit { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual Registration Registration { get; set; }
        public virtual ICollection<SellingAdjustment> SellingAdjustment { get; set; }
        public virtual ICollection<SellingList> SellingList { get; set; }
        public virtual ICollection<SellingPaymentList> SellingPaymentList { get; set; }
    }
}
