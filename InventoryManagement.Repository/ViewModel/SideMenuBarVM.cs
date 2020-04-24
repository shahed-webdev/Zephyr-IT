using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class SideMenuCategory
    {
        public SideMenuCategory()
        {
            this.links = new HashSet<SideMenuLink>();
        }
        public int LinkCategoryId { get; set; }
        public int? Sn { get; set; }
        public string Category { get; set; }
        public string IconClass { get; set; }
        public ICollection<SideMenuLink> links { get; set; }

    }
    public class SideMenuLink
    {
        public int LinkId { get; set; }
        public int? Sn { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }

    }
}
