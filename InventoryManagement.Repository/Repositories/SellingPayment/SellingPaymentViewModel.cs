using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class SellingPaymentViewModel
    {
        public string PaymentMethod { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal AccountTransactionCharge { get; set; }
        public DateTime PaidDate { get; set; }
    }

    public class SellingDuePaySingleModel
    {
        public int SellingId { get; set; }
        public int CustomerId { get; set; }
        public int RegistrationId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal AccountTransactionCharge { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public DateTime PaidDate { get; set; }
        public int? AccountId { get; set; }
    }

    public class SellingDuePayMultipleModel
    {
        public SellingDuePayMultipleModel()
        {
            Bills = new HashSet<SellingDuePayMultipleBill>();
        }
        public int CustomerId { get; set; }
        public int RegistrationId { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal AccountTransactionCharge { get; set; }
        public int? AccountId { get; set; }
        public DateTime PaidDate { get; set; }
        public ICollection<SellingDuePayMultipleBill> Bills { get; set; }

        public void CalculateEachBillTransactionCharge()
        {
            var amount = PaidAmount - AccountTransactionCharge;

            var percentage = (AccountTransactionCharge * 100) / amount;

            foreach (var bill in Bills)
            {
                bill.AccountTransactionCharge = (percentage / 100) * bill.SellingPaidAmount;
                bill.SellingPaidAmount += bill.AccountTransactionCharge;
            }
        }
    }

    public class SellingDuePayMultipleBill
    {
        public int SellingId { get; set; }
        public decimal SellingPaidAmount { get; set; }
        public decimal AccountTransactionCharge { get; set; }

    }

    public class SellingPaymentRecordModel
    {
        public int SellingPaymentId { get; set; }
        public int CustomerId { get; set; }
        public int RegistrationId { get; set; }
        public string CollectBy { get; set; }
        public string CustomerName { get; set; }
        public int ReceiptSn { get; set; }
        public int SellingId { get; set; }
        public int SellingSn { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaidDate { get; set; }
    }

    public class PaymentCollectionByAccount
    {
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
    }
}