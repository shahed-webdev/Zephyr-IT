using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class PurchaseViewModel
    {
        public PurchaseViewModel()
        {
            this.Products = new HashSet<ProductViewModel>();
        }

        public int PurchaseId { get; set; }
        public int RegistrationId { get; set; }
        public int VendorId { get; set; }
        public double PurchaseTotalPrice { get; set; }
        public double PurchaseDiscountAmount { get; set; }
        public double PurchasePaidAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }
    }
}