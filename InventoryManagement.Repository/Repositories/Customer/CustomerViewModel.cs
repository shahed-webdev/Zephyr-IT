using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Repository
{
    public class CustomerAddUpdateViewModel
    {
        public CustomerAddUpdateViewModel()
        {
            PhoneNumbers = new HashSet<CustomerPhoneViewModel>();
        }
        public int CustomerId { get; set; }
        public string OrganizationName { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Description { get; set; }
        public double DueLimit { get; set; }
        public string Designation { get; set; }
        public bool IsIndividual { get; set; }
        public byte[] Photo { get; set; }
        [Required]
        public ICollection<CustomerPhoneViewModel> PhoneNumbers { get; set; }
    }

    public class CustomerPhoneViewModel
    {
        public int? CustomerPhoneId { get; set; }
        [Required]
        public string Phone { get; set; }
        public bool? IsPrimary { get; set; }
    }

    public class CustomerListViewModel
    {
        public int CustomerId { get; set; }
        public string OrganizationName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PhonePrimary { get; set; }
        public double Due { get; set; }
        public double DueLimit { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public bool IsIndividual { get; set; }
        public DateTime SignUpDate { get; set; }
    }

    public class CustomerReceiptViewModel
    {
        public int CustomerId { get; set; }
        public string OrganizationName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
    }

    public class CustomerProfileViewModel
    {
        public CustomerProfileViewModel()
        {
            PhoneNumbers = new HashSet<CustomerPhoneViewModel>();
            SellingRecords = new HashSet<CustomerSellingViewModel>();
        }
        public int CustomerId { get; set; }
        public string OrganizationName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Description { get; set; }
        public double DueLimit { get; set; }
        public string Designation { get; set; }
        public bool IsIndividual { get; set; }
        public byte[] Photo { get; set; }
        public DateTime SignUpDate { get; set; }
        public ICollection<CustomerPhoneViewModel> PhoneNumbers { get; set; }
        public ICollection<CustomerSellingViewModel> SellingRecords { get; set; }
        public double SoldAmount { get; set; }
        public double ReceivedAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double DueAmount { get; set; }
    }
    public class CustomerSellingViewModel
    {
        public int SellingId { get; set; }
        public int SellingSn { get; set; }
        public double SellingAmount { get; set; }
        public double SellingPaidAmount { get; set; }
        public double SellingDiscountAmount { get; set; }
        public double SellingDueAmount { get; set; }
        public DateTime SellingDate { get; set; }
    }
}
