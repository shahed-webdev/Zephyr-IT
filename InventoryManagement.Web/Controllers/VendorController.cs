using System;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        private readonly IUnitOfWork _db;

        public VendorController(IUnitOfWork db)
        {
            _db = db;
        }

        #region Vendor list
        // GET: List
        [Authorize(Roles = "admin, vendor-list")]
        public IActionResult List()
        {
            return View();
        }

        public IActionResult ListDataTable(DataRequest request)
        {
            var data = _db.Vendors.ToListDataTable(request);
            return Json(data);
        }
        #endregion

        #region Vendor CRUD
        // GET: Vendors/Create
        [Authorize(Roles = "admin, vendor")]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: Vendors/Create
        [Authorize(Roles = "admin, vendor")]
        [HttpPost]
        public async Task<IActionResult> Create(VendorViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Create", model);

            var vendor = _db.Vendors.AddCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0)
            {
                model.VendorId = vendor.VendorId;
                var result = new AjaxContent<VendorViewModel> { Status = true, Data = model };
                return Json(result);
            }

            ModelState.AddModelError("", "Unable to insert record!");
            return PartialView("_Create", model);
        }

        // GET: Vendors/Edit/5
        [Authorize(Roles = "admin, vendor")]
        public ActionResult Edit(int? id)
        {
            if (id == null) return BadRequest(HttpStatusCode.BadRequest);

            var model = _db.Vendors.FindCustom(id);
            if (model == null) return NotFound();

            return PartialView("_Edit", model);
        }

        // POST: Vendors/Edit/5
        [Authorize(Roles = "admin, vendor")]
        [HttpPost]
        public async Task<IActionResult> Edit(VendorViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Edit", model);

            _db.Vendors.UpdateCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0) return Content("success");

            ModelState.AddModelError("", "Unable to update");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return PartialView("_Edit", model);
        }

        // POST: Delete/5
        [Authorize(Roles = "admin, vendor")]
        public int Delete(int id)
        {
            if (!_db.Vendors.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
        }
        #endregion

        #region Vendor Details

        //GET:// Details
        public IActionResult Details(int? id, DateTime? from, DateTime? to)
        {
            if (!id.HasValue) return RedirectToAction("List");

            var model = _db.Vendors.ProfileDetails(id.GetValueOrDefault(), from,to);
            if (model == null) return RedirectToAction("List");

            return View(model);
        }

        //get details by date
        [HttpPost]
        public IActionResult DetailsByDates(int id, DateTime from, DateTime to)
        {
            var response = _db.Vendors.ProfileDetails(id, from, to); ;
            return Json(response);
        }

        //vendor purchase data-table(ajax)
        [HttpPost]
        public IActionResult PurchaseRecordsData(DataRequest request)
        {
            var data = _db.Purchases.Records(request);
            return Json(data);
        }

        //payment
        [HttpPost]
        public IActionResult PurchasePaymentRecordsData(DataRequest request)
        {
            var data = _db.PurchasePayments.Records(request);
            return Json(data);
        }

        #endregion
    }
}