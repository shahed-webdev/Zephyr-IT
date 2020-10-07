﻿using System;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
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

        public IActionResult Index(int? year)
        {
            var d = new DashboardRepository(_db);
            return View(d.Data(year));
        }

        public IActionResult TopDueCustomer(DataRequest request)
        {
            var data = _db.Customers.TopDueDataTable(request);
            return Json(data);
        }

        public IActionResult TopDueVendor(DataRequest request)
        {
            var data = _db.Vendors.TopDueDataTable(request);
            return Json(data);
        }

        //find by date
        public IActionResult DailyReport(DateTime date)
        {
            var data = new DashboardRepository(_db);
           var response = data.DailyData(date);

            return Json(response);
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
            if (!ModelState.IsValid) return RedirectToAction("Index");

            _db.Institutions.UpdateCustom(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Login Info
        public string GetUserLoggedInInfo()
        {
            var admin = _db.Registrations.GetAdminBasic(User.Identity.Name);
            return JsonConvert.SerializeObject(admin); //Serialize for image binary data
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