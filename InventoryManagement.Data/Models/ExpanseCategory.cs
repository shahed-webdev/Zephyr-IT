using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ExpanseCategory
    {
        public ExpanseCategory()
        {
            Expanse = new HashSet<Expanse>();
        }

        public int ExpanseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public double TotalExpanse { get; set; }

        public virtual ICollection<Expanse> Expanse { get; set; }
    }
}
