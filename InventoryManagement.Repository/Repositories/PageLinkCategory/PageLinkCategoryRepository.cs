using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class PageLinkCategoryRepository : Repository<PageLinkCategory>, IPageLinkCategoryRepository
    {
        public PageLinkCategoryRepository(ApplicationDbContext context) : base(context)
        {

        }

        public ICollection<PageLinkCategoryViewModel> GetCategoryWithLink()
        {
            return Context.PageLinkCategory.Include(p => p.PageLink).OrderBy(p => p.Sn).Select(c => new PageLinkCategoryViewModel
            {
                LinkCategoryId = c.LinkCategoryId,
                Category = c.Category,
                PageLinks = c.PageLink.Select(p => new PageLinkViewModel
                {
                    LinkId = p.LinkId,
                    LinkCategoryId = p.LinkCategoryId,
                    RoleId = p.RoleId,
                    Controller = p.Controller,
                    Action = p.Action,
                    Title = p.Title,
                    IconClass = p.IconClass,
                    Sn = p.Sn
                }).ToList()
            }).ToList();
        }

        public PageLink LinkRoleUpdate(int linkId, string roleId)
        {
            var linkage = Context.PageLink.Find(linkId);
            linkage.RoleId = roleId;
            return linkage;
        }

        public ICollection<DDL> ddl()
        {
            return Context.PageLinkCategory.Select(c => new DDL
            {
                value = c.LinkCategoryId,
                label = c.Category
            }).ToList();
        }
    }
}