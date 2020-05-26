using InventoryManagement.Data;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class PageLinkAssignRepository : Repository<PageLinkAssign>, IPageLinkAssignRepository
    {
        public PageLinkAssignRepository(ApplicationDbContext context) : base(context)
        {
        }

        public string AssignLink(int regId, ICollection<PageAssignVM> links)
        {
            var pAssigns = links.Select(l => new PageLinkAssign
            {
                LinkId = l.LinkId,
                RegistrationId = regId
            }).ToList();

            var pDelete = Context.PageLinkAssign.Where(p => p.RegistrationId == regId).ToList();
            Context.PageLinkAssign.RemoveRange(pDelete);
            Context.PageLinkAssign.AddRange(pAssigns);
            return Context.Registration.Find(regId).UserName;
        }

        public ICollection<PageCategoryVM> SubAdminLinks(int regId)
        {
            var userDll = (from c in Context.PageLinkCategory
                           select new PageCategoryVM
                           {
                               Category = c.Category,
                               Links = (from l in Context.PageLink
                                        join r in Context.Roles
                                        on l.RoleId equals r.Id
                                        where l.LinkCategoryId == c.LinkCategoryId
                                        select new PageLinkVM
                                        {
                                            LinkId = l.LinkId,
                                            Title = l.Title,
                                            RoleName = r.Name,
                                            IsAssign = (from a in Context.PageLinkAssign where a.LinkId == l.LinkId && a.RegistrationId == regId select a.LinkAssignId).Any()
                                        }).ToList()
                           }).ToList();
            return userDll;
        }
    }
}
