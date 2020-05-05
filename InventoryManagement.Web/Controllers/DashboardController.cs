using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryManagement.Web.Controllers
{
   [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _db;

        public DashboardController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET: Profile
        public IActionResult Profile()
        {
            var user = _db.Registrations.GetAdminInfo(User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        public IActionResult Profile(AdminInfo user)
        {
            if (!ModelState.IsValid) return View(user);

            _db.Registrations.UpdateCustom(User.Identity.Name, user);
            _db.SaveChanges();

            return RedirectToAction("Index", new { Message = "Profile information Updated" });
        }

        //GET: Store Info
        public IActionResult StoreInfo()
        {
            var model = _db.Institutions.FindCustom();
            return View(model);
        }
       
        [HttpPost]
        public IActionResult StoreInfo(InstitutionVM model)
        {
            if (ModelState.IsValid)
            {
                _db.Institutions.UpdateCustom(model);
                _db.SaveChanges();
            }

            return RedirectToAction($"Index", $"Dashboard", new { Message = "Store information Updated" });
        }

        //Login Info
        [Authorize(Roles = "admin, sub-admin")]
        public string GetUserLoggedInInfo()
        {
            var admin = _db.Registrations.GetAdminBasic(User.Identity.Name);
            return JsonConvert.SerializeObject(admin); //Serialize for image binary data
        }
    }
}