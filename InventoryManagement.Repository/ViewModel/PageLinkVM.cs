using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class PageLinkVM
    {
        public int LinkId { get; set; }
        public bool IsAssign { get; set; } = false;
        public string Title { get; set; }
        public string RoleName { get; set; }
    }

    public class PageCategoryVM
    {
        public PageCategoryVM()
        {
            this.Links = new HashSet<PageLinkVM>();
        }
        public string Category { get; set; }

        public ICollection<PageLinkVM> Links { get; set; }
    }

    public class PageAssignVM
    {
        public int LinkId { get; set; }
        public string RoleName { get; set; }
    }
}
