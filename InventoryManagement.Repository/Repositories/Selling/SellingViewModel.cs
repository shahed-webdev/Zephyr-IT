using System;

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

    }

    public class SellingRecordViewModel
    {

    }
}