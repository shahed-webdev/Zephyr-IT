using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Repository
{

    public class VendorViewModel
    {
        public int VendorId { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string VendorCompanyName { get; set; }
        [Display(Name = "Name")]
        public string VendorName { get; set; }
        [Display(Name = "Address")]
        public string VendorAddress { get; set; }
        [Required]
        [Display(Name = "Phone")]
        public string VendorPhone { get; set; }
        public string Description { get; set; }
        [Display(Name = "Add Date")]
        public DateTime InsertDate { get; set; }
        public double Due { get; set; }
    }

    public class VendorPaidDue
    {
        public int VendorId { get; set; }
        public string VendorCompanyName { get; set; }
        public double VendorDue { get; set; }
        public double VendorPaid { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
    }
}
