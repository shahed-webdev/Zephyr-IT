using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ProductCatalog
    {
        public ProductCatalog()
        {
            InverseParent = new HashSet<ProductCatalog>();
            Product = new HashSet<Product>();
            ServiceDevice = new HashSet<ServiceDevice>();
            Warranty = new HashSet<Warranty>();
        }

        public int ProductCatalogId { get; set; }
        public int? CatalogTypeId { get; set; }
        public string CatalogName { get; set; }
        public int CatalogLevel { get; set; }
        public int? ParentId { get; set; }
        public int ItemCount { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual ProductCatalogType CatalogType { get; set; }
        public virtual ProductCatalog Parent { get; set; }
        public virtual ICollection<ProductCatalog> InverseParent { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<ServiceDevice> ServiceDevice { get; set; }
        public virtual ICollection<Warranty> Warranty { get; set; }
    }
}
