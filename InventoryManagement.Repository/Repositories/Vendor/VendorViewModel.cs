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
        public decimal Due { get; set; }

        [Display(Name = "Add Date")]
        public DateTime InsertDate { get; set; }
    }

    public class VendorPaidDue
    {
        public int VendorId { get; set; }
        public string VendorCompanyName { get; set; }
        public decimal VendorDue { get; set; }
        public decimal VendorPaid { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
    }

    public class VendorLedger
    {
        public DateTime ActivityDate { get; set; }
        public string Particulars { get; set; }
        public decimal Amount { get; set; }
    }
    public class VendorProfileViewModel
    {
        public int VendorId { get; set; }
        public string VendorCompanyName { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorPhone { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal ReturnAmount { get; set; }
        public decimal Paid { get; set; }
        public decimal Due { get; set; }
        public byte[] Photo { get; set; }
        public DateTime SignUpDate { get; set; }
    }
}
