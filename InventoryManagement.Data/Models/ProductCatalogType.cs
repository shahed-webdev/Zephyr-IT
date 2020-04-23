using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class ProductCatalogType
    {
        public ProductCatalogType()
        {
            ProductCatalog = new HashSet<ProductCatalog>();
        }

        public int CatalogTypeId { get; set; }
        public string CatalogType { get; set; }
        public DateTime InsertDate { get; set; }

        public virtual ICollection<ProductCatalog> ProductCatalog { get; set; }
    }
}
