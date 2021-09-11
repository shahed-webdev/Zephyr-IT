using System;
using System.Threading.Tasks;
using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryManagement.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IAccountCore _account;
        public DashboardController(IUnitOfWork db, IAccountCore account)
        {
            _db = db;
            _account = account;
        }

        [Authorize(Roles = "admin, SubAdmin")]
        public IActionResult Index(int? year)
        {
            var d = new DashboardRepository(_db);
            ViewBag.Capital = _account.CapitalGet();
            
            return View(d.Data(year));
        }

        [Authorize(Roles = "admin, SubAdmin")]
        public IActionResult TopDueCustomer(DataRequest request)
        {
            var data = _db.Customers.TopDueDataTable(request);
            return Json(data);
        }

        [Authorize(Roles = "admin, SubAdmin")]
        public IActionResult TopDueVendor(DataRequest request)
        {
            var data = _db.Vendors.TopDueDataTable(request);
            return Json(data);
        }

        //find by date
        [Authorize(Roles = "admin, SubAdmin")]
        public IActionResult DailyReport(DateTime date)
        {
            var data = new DashboardRepository(_db);
           var response = data.DailyData(date);

            return Json(response);
        }

        //GET: Profile
        [Authorize(Roles = "admin, SubAdmin, SalesPerson")]
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

            return RedirectToAction("Profile", new { Message = "Profile information Updated" });
        }

        //GET: Store Info
        [Authorize(Roles = "admin, SubAdmin")]
        public IActionResult StoreInfo()
        {
            var model = _db.Institutions.FindCustom();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin, SubAdmin")]
        public IActionResult StoreInfo(InstitutionVM model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            _db.Institutions.UpdateCustom(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Login Info
        [Authorize(Roles = "admin, SubAdmin, SalesPerson")]
        public string GetUserLoggedInInfo()
        {
            var admin = _db.Registrations.GetAdminBasic(User.Identity.Name);
            return JsonConvert.SerializeObject(admin); //Serialize for image binary data
        }



        //promise date
        [HttpPost]
        public IActionResult GetPromiseDateData(DataRequest request)
        {
            var data = _db.Selling.DueRecords(request,true);
            return Json(data);
        }


        [HttpPost]
       public async Task<IActionResult> PromiseDateUpdate(int id, DateTime newDate)
       {
           var registrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
           var response = await _db.Selling.PromiseDateChange(id, newDate, registrationId);
           
           return Json(response);
       }
    }
}