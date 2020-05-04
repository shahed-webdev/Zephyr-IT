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
        
        public string VendorName { get; set; }
      
        public string VendorAddress { get; set; }
        
        [Required]
        public string VendorPhone { get; set; }

        public string Description { get; set; }
        public double Due { get; set; }
       
        [Display(Name = "Add Date")]
        public DateTime InsertDate { get; set; }
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
