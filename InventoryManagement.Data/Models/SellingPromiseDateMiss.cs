using System;

namespace InventoryManagement.Data
{
    public class SellingPromiseDateMiss
    {
        public int SellingPromiseDateMissId { get; set; }
        public int RegistrationId { get; set; }
        public int SellingId { get; set; }
        public DateTime MissDate { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public Selling Selling { get; set; }
        public Registration Registration { get; set; }

    }
}