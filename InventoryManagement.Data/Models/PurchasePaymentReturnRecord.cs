using System;

namespace InventoryManagement.Data
{
    public class PurchasePaymentReturnRecord
    {
        public int PurchasePaymentReturnRecordId { get; set; }
        public decimal PrevReturnAmount { get; set; }
        public decimal CurrentReturnAmount { get; set; }
        public decimal NetReturnAmount { get; set; }
        public int? AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public int RegistrationId { get; set; }
        public Registration Registration { get; set; }
    }
}