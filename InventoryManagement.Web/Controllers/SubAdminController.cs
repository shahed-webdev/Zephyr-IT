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
        [Authorize(Roles = "admin,SubAdmin_List")]
        public IActionResult List()
        {
            var model = _db.Registrations.GetSubAdminList();
            return View(model);
        }

        //
        // GET: /SignUp
        [Authorize(Roles = "admin, sub-admin")]
        public IActionResult SignUp()
        {
            return View();
        }

        //
        // POST: /SignUp
        [HttpPost]
        [Authorize(Roles = "admin, sub-admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _userManager.AddToRoleAsync(user, "sub-admin").ConfigureAwait(false);

                var reg = new Registration()
                {
                    Type = "sub-admin",
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

        [Authorize(Roles = "admin,SubAdmin_PageAccess")]
        public IActionResult PageAccess()
        {
            ViewBag.SubAdmins = new SelectList(_db.Registrations.SubAdmins(), "value", "label");
            var model = _db.PageLinkAssigns.SubAdminLinks(0);
            return View(model);
        }

        public IActionResult GetLinks(int regId)
        {
            var model = _db.PageLinkAssigns.SubAdminLinks(regId);
            return Json(model);
        }

        public async Task<bool> PostLinks(int regId, ICollection<PageAssignVM> links)
        {
            try
            {
                var userName = _db.PageLinkAssigns.AssignLink(regId, links);

                await _db.SaveChangesAsync().ConfigureAwait(false);

                var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
                var roleList = links.Select(l => l.RoleName).ToList();

                roleList.Add("Sub-Admin");

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
    }
}