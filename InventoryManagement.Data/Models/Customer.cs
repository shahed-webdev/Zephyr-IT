using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Customer
    {
        public Customer()
        {
            Selling = new HashSet<Selling>();
            SellingPayment = new HashSet<SellingPayment>();
            Service = new HashSet<Service>();
            ServiceDevice = new HashSet<ServiceDevice>();
            ExpenseTransportation = new HashSet<ExpenseTransportation>();
        }

        public int CustomerId { get; set; }
        public string OrganizationName { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public string CustomerAddress { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double ReturnAmount { get; set; }
        public double Paid { get; set; }
        public double Due { get; set; }
        public double DueLimit { get; set; }
        public byte[] Photo { get; set; }
        public DateTime InsertDate { get; set; }
        public string Designation { get; set; }
        public bool IsIndividual { get; set; }

        public virtual ICollection<Selling> Selling { get; set; }
        public virtual ICollection<SellingPayment> SellingPayment { get; set; }
        public virtual ICollection<Service> Service { get; set; }
        public virtual ICollection<ServiceDevice> ServiceDevice { get; set; }
        public virtual ICollection<CustomerPhone> CustomerPhone { get; set; }
        public virtual ICollection<ExpenseTransportation> ExpenseTransportation { get; set; }
    }
}
