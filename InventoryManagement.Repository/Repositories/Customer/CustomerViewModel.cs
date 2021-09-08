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
        public decimal DueLimit { get; set; }
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
        public decimal Due { get; set; }
        public decimal DueLimit { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public bool IsIndividual { get; set; }
        public DateTime SignUpDate { get; set; }
    }
    public class CustomerReceiptViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
    }
    public class CustomerProfileViewModel
    {
        public CustomerProfileViewModel()
        {
            PhoneNumbers = new HashSet<CustomerPhoneViewModel>();
        }
        public int CustomerId { get; set; }
        public string OrganizationName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Description { get; set; }
        public decimal DueLimit { get; set; }
        public string Designation { get; set; }
        public bool IsIndividual { get; set; }
        public byte[] Photo { get; set; }
        public DateTime SignUpDate { get; set; }
        public ICollection<CustomerPhoneViewModel> PhoneNumbers { get; set; }
        public decimal SoldAmount { get; set; }
        public decimal AccountTransactionCharge { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DueAmount { get; set; }
    }
    public class CustomerSellingViewModel
    {
        public int CustomerId { get; set; }
        public int SellingId { get; set; }
        public int SellingSn { get; set; }
        public decimal SellingTotalPrice { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal SellingPaidAmount { get; set; }
        public decimal AccountTransactionCharge { get; set; }
        public decimal SellingReturnAmount { get; set; }
        public decimal SellingDiscountAmount { get; set; }
        public decimal SellingDueAmount { get; set; }
        public DateTime SellingDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime? PromisedPaymentDate { get; set; }
    }

    public class CustomerDueViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public decimal Due { get; set; }
    }

    public class CustomerMultipleDueCollectionViewModel
    {
        public CustomerMultipleDueCollectionViewModel()
        {
            SellingDueRecords = new HashSet<CustomerSellingViewModel>();
        }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public decimal TotalDue { get; set; }
        public ICollection<CustomerSellingViewModel> SellingDueRecords { get; set; }
    }
}
