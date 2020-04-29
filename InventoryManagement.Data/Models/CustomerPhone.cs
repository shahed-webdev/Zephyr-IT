using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Data
{
   public class CustomerPhone
    {
        public int CustomerPhoneId { get; set; }
        public int CustomerId { get; set; }
        public string Phone { get; set; }
        public bool? IsPrimary { get; set; }
        public DateTime InsertDate { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
