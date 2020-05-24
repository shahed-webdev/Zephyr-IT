using InventoryManagement.Data;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IPageLinkRepository : IRepository<PageLink>, IAddCustom<PageLinkViewModel>
    {
        List<SideMenuCategory> GetSideMenuByUser(string userName);
    }
}
