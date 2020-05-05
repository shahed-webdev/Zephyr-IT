using System.ComponentModel.DataAnnotations;

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
        public int ParentId { get; set; }
    }
}