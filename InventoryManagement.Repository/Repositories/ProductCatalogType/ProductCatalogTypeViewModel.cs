using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Repository
{
    public class ProductCatalogTypeViewModel
    {
        public int CatalogTypeId { get; set; }
        
        [Required]
        [Display(Name = "Catalog Type")]
        public string CatalogType { get; set; }
    }
}