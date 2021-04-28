using System;

namespace InventoryManagement.Repository
{
    public class AdminMoneyCollectionAddModel
    {
        public int AdminMoneyCollectionId { get; set; }
        public int RegistrationId { get; set; }
        public decimal CollectionAmount { get; set; }
        public string Description { get; set; }
    }

    public class AdminMoneyCollectionViewModel
    {
        public int AdminMoneyCollectionId { get; set; }
        public int RegistrationId { get; set; }
        public decimal CollectionAmount { get; set; }
        public string Description { get; set; }
        public string CollectionFrom { get; set; }
        public DateTime InsertDate { get; set; }
    }
}