using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class PageLinkCategoryViewModel
    {
        public PageLinkCategoryViewModel()
        {
            PageLinks = new HashSet<PageLinkViewModel>();
        }

        public int LinkCategoryId { get; set; }
        public string Category { get; set; }
        public ICollection<PageLinkViewModel> PageLinks { get; set; }
    }
}