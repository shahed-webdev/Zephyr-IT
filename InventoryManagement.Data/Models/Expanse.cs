using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Expanse
    {
        public int ExpanseId { get; set; }
        public int RegistrationId { get; set; }
        public int ExpanseCategoryId { get; set; }
        public double ExpanseAmount { get; set; }
        public string ExpanseFor { get; set; }
        public string ExpansePaymentMethod { get; set; }
        public DateTime ExpanseDate { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual ExpanseCategory ExpanseCategory { get; set; }
        public virtual Registration Registration { get; set; }
    }
}
