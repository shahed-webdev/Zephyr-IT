using InventoryManagement.Data;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    internal class PageLinkRepository : Repository<PageLink>, IPageLinkRepository
    {
        public PageLinkRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<SideMenuCategory> GetSideMenuByUser(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            var reg = Context.Registration.FirstOrDefault(r => r.UserName == userName);
            if (reg.Type == "Admin")
            {
                var menu = (from p in Context.PageLinkCategory
                            orderby p.Sn
                            select new SideMenuCategory
                            {
                                LinkCategoryId = p.LinkCategoryId,
                                Category = p.Category,
                                IconClass = p.IconClass,
                                Sn = p.Sn,
                                links = p.PageLink.Select(l => new SideMenuLink
                                {
                                    LinkId = l.LinkId,
                                    Sn = l.Sn,
                                    Action = l.Action,
                                    Controller = l.Controller,
                                    IconClass = l.IconClass,
                                    Title = l.Title
                                }).OrderBy(l => l.Sn).ToList()
                            }).ToList();
                return menu;
            }
            else
            {
                var menu = (from p in Context.PageLinkAssign
                            join c in Context.PageLinkCategory
                                on p.Link.LinkCategory.LinkCategoryId equals c.LinkCategoryId
                            where p.RegistrationId == reg.RegistrationId
                            orderby p.Link.LinkCategory.Sn
                            select new SideMenuCategory
                            {
                                LinkCategoryId = c.LinkCategoryId,
                                Category = c.Category,
                                IconClass = c.IconClass,
                                Sn = c.Sn
                            }).Distinct().ToList();

                foreach (var item in menu)
                {
                    item.links = Context.PageLinkAssign
                        .Where(l => l.Link.LinkCategoryId == item.LinkCategoryId &&
                                    l.RegistrationId == reg.RegistrationId).Select(l => new SideMenuLink
                                    {
                                        LinkId = l.Link.LinkId,
                                        Sn = l.Link.Sn,
                                        Action = l.Link.Action,
                                        Controller = l.Link.Controller,
                                        IconClass = l.Link.IconClass,
                                        Title = l.Link.Title
                                    }).ToList();
                }

                return menu;
            }
        }

        public void AddCustom(PageLinkViewModel model)
        {
            var pageLink = new PageLink
            {
                LinkCategoryId = model.LinkCategoryId,
                RoleId = model.RoleId,
                Controller = model.Controller,
                Action = model.Action,
                Title = model.Title,
                IconClass = model.IconClass,
                Sn = model.Sn
            };
            Add(pageLink);
        }
    }
}