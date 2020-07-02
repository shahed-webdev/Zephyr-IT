using InventoryManagement.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class ProductCatalogViewModel
    {
        public int ProductCatalogId { get; set; }

        [Display(Name = "Catalog Type")]
        public int? CatalogTypeId { get; set; }

        [Required]
        [Display(Name = "Catalog Name")]
        public string CatalogName { get; set; }

        [Display(Name = "Parent Catalog")]
        public int? ParentId { get; set; }
    }


    public class ProductCatalogShow
    {
        public ProductCatalogShow()
        {
            SubCatalog = new HashSet<ProductCatalogShow>();
        }
        public ProductCatalogShow(ProductCatalog catalog)
        {
            CatalogName = catalog.CatalogName;
            ProductCatalogId = catalog.ProductCatalogId;
            SubCatalog = catalog.InverseParent.Select(c => new ProductCatalogShow(c));
        }
        public int ProductCatalogId { get; set; }
        public string CatalogName { get; set; }
        public IEnumerable<ProductCatalogShow> SubCatalog { get; set; }
    }


    public class ProductCatalogUpdateViewModel
    {
        public int ProductCatalogId { get; set; }
        public string CatalogName { get; set; }
    }

}