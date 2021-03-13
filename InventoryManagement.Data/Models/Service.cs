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
        public decimal ServiceTotalPrice { get; set; }
        public decimal ServiceDiscountAmount { get; set; }
        public decimal ServiceDiscountPercentage { get; set; }
        public decimal ServicePaidAmount { get; set; }
        public decimal ServiceDueAmount { get; set; }
        public string ServicePaymentStatus { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Registration Registration { get; set; }
        public virtual ICollection<ServiceList> ServiceList { get; set; }
        public virtual ICollection<ServicePaymentList> ServicePaymentList { get; set; }
    }
}
