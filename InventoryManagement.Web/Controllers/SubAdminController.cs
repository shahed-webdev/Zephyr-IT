using InventoryManagement.Data;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class SubAdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<SubAdminController> _logger;
        private readonly IUnitOfWork _db;

        public SubAdminController(UserManager<IdentityUser> userManager, ILogger<SubAdminController> logger, IUnitOfWork db)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
        }

        [Authorize(Roles = "admin, sub-admin-list")]
        public IActionResult List()
        {
            var model = _db.Registrations.GetSubAdminList();
            return View(model);
        }

        // GET: /SignUp
        [Authorize(Roles = "admin, sub-admin-signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: /SignUp
        [Authorize(Roles = "admin, sub-admin-signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _userManager.AddToRoleAsync(user, model.Type.ToString()).ConfigureAwait(false);

                var reg = new Registration()
                {
                    Type = model.Type.ToString(),
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email,
                    Address = model.Address,
                    Ps = model.Password,
                    Phone = model.Phone
                };

                _db.Registrations.Add(reg);
                await _db.SaveChangesAsync().ConfigureAwait(false);

                return RedirectToAction("List", "SubAdmin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //page access
        [Authorize(Roles = "admin, sub-admin-page-access")]
        public IActionResult PageAccess(int? id)
        {
            if (!id.HasValue) return RedirectToAction("List");

            var model = _db.PageLinkAssigns.SubAdminLinks(id.GetValueOrDefault());
            return View(model);
        }

        [HttpPost]
        public async Task<bool> PostLinks(int regId, ICollection<PageAssignVM> links)
        {
            try
            {
                var userName = _db.PageLinkAssigns.AssignLink(regId, links);

                await _db.SaveChangesAsync().ConfigureAwait(false);

                var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
                var roleList = links.Select(l => l.RoleName).ToList();

                roleList.Add(UserType.SubAdmin.ToString());

                var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray()).ConfigureAwait(false);
                await _userManager.AddToRolesAsync(user, roleList.ToArray()).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}