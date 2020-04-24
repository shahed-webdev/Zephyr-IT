using InventoryManagement.Data;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IPageLinkRepository : IRepository<PageLink>
    {
        List<SideMenuCategory> GetSideMenuByUser(string userName);
    }
}
