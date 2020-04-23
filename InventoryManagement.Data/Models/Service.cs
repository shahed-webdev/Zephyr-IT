using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Service
    {
        public Service()
        {
            ServiceList = new HashSet<ServiceList>();
            ServicePaymentList = new HashSet<ServicePaymentList>();
        }

        public int ServiceId { get; set; }
        public int RegistrationId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceSn { get; set; }
        public double ServiceTotalPrice { get; set; }
        public double ServiceDiscountAmount { get; set; }
        public double ServiceDiscountPercentage { get; set; }
        public double ServicePaidAmount { get; set; }
        public double ServiceDueAmount { get; set; }
        public string ServicePaymentStatus { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Registration Registration { get; set; }
        public virtual ICollection<ServiceList> ServiceList { get; set; }
        public virtual ICollection<ServicePaymentList> ServicePaymentList { get; set; }
    }
}
