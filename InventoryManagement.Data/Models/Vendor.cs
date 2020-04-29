﻿using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Vendor
    {
        public Vendor()
        {
            Purchase = new HashSet<Purchase>();
            PurchasePayment = new HashSet<PurchasePayment>();
        }

        public int VendorId { get; set; }
        public string VendorCompanyName { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorPhone { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double ReturnAmount { get; set; }
        public double Paid { get; set; }
        public double Due { get; set; }
        public byte[] Photo { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual ICollection<Purchase> Purchase { get; set; }
        public virtual ICollection<PurchasePayment> PurchasePayment { get; set; }
    }
}