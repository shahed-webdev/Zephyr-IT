using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Repository
{
    public class PageLinkViewModel
    {
        public int LinkId { get; set; }
        [Required]
        public int LinkCategoryId { get; set; }
        public string RoleId { get; set; }
        [Required]
        public string Controller { get; set; }
        [Required]
        public string Action { get; set; }
        [Required]
        public string Title { get; set; }
        public string IconClass { get; set; }
        public int? Sn { get; set; }
    }
}