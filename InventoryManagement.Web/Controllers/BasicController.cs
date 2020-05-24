using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class BasicController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _db;
        public BasicController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUnitOfWork db, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _roleManager = roleManager;

        }


        //Side Menu
        [Authorize(Roles = "admin, sub-admin")]
        public string GetSideMenu()
        {
            var data = _db.PageLinks.GetSideMenuByUser(User.Identity.Name);
            return JsonConvert.SerializeObject(data);
        }


        /******PAGE ACCESS ROLE********/
        public ActionResult PageRole()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        //GET
        public ActionResult CreateRole()
        {
            var role = new IdentityRole();
            return View(role);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(IdentityRole role)
        {
            if (role.Name == null) return View();
            if (await _roleManager.RoleExistsAsync(role.Name).ConfigureAwait(false)) return View();
            await _roleManager.CreateAsync(role).ConfigureAwait(false);

            return RedirectToAction("PageRole");
        }

        public async Task<bool> DeleteRole(string roleName)
        {
            if (roleName == null) return false;

            var role = await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
            var userInRole = await _userManager.GetUsersInRoleAsync(roleName).ConfigureAwait(false);
            if (userInRole.Count > 0) return false;

            var r = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
            return r.Succeeded;
        }

        // Page Links
        public ActionResult PageLink()
        {
            ViewBag.roleList = _roleManager.Roles.Select(r => new RoleDDL { RoleId = r.Id, Role = r.Name }).ToList();

            var model = _db.PageLinkCategories.GetCategoryWithLink();
            return View(model);
        }

        public ActionResult CreateLinks()
        {
            ViewBag.roleList = _roleManager.Roles.Select(r => new RoleDDL { RoleId = r.Id, Role = r.Name }).ToList();
            ViewBag.Category = _db.PageLinkCategories.ddl();
            return View();
        }

        [HttpPost]
        public ActionResult CreateLinks(PageLinkViewModel model)
        {
            if (!ModelState.IsValid) return View();

            _db.PageLinks.AddCustom(model);
            _db.SaveChanges();

            return RedirectToAction("PageLink");
        }

        public bool PageLinkUpdate(int linkId, string roleId)
        {
            var linkage = _db.PageLinkCategories.LinkRoleUpdate(linkId, roleId);
            _db.PageLinks.Update(linkage);
            var r = _db.SaveChanges();

            return r > 0;
        }
    }
}
