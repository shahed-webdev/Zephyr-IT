using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagement.BusinessLogin;

namespace InventoryManagement.Web.Views
{
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IAccountCore _account;
        public PurchaseController(IUnitOfWork db, IAccountCore account)
        {
            _db = db;
            _account = account;
        }

        //GET: Purchase
        [Authorize(Roles = "admin, purchase")]
        public IActionResult Purchase()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");

            return View();
        }

        //POST: Purchase
        [Authorize(Roles = "admin, purchase")]
        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);

            if (!ModelState.IsValid) UnprocessableEntity(ModelState);

            var response = await _db.Purchases.AddCustomAsync(model, _db).ConfigureAwait(false);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseCodeIsExist([FromBody] List<ProductStockViewModel> stocks)
        {
            var existList = await _db.ProductStocks.IsExistListAsync(stocks).ConfigureAwait(false);
            return Json(existList);
        }

        //GET: find vendor from ajax autocomplete
        public async Task<IActionResult> FindVendor(string prefix)
        {
            var data = await _db.Vendors.SearchAsync(prefix).ConfigureAwait(false);
            return Json(data);
        }


        public async Task<IActionResult> PurchaseReceipt(int? id)
        {
            if (id == null) return RedirectToAction("Purchase");

            var model = await _db.Purchases.PurchaseReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);

            if (model == null) return RedirectToAction("Purchase");
            return View(model);
        }

        //purchase Records
        [Authorize(Roles = "admin, purchase-record")]
        public IActionResult PurchaseRecords()
        {
            return View();
        }

        //request from data-table(ajax)
        public IActionResult PurchaseRecordsData(DataRequest request)
        {
            var data = _db.Purchases.Records(request);
            return Json(data);
        }

        //memo number update
        public async Task<IActionResult> MemoUpdate(int id, string newMemoNo)
        {
            var response = await _db.Purchases.UpdateMemoNumberAsync(id, newMemoNo);
            if (response.IsSuccess) return Ok(response);

            return UnprocessableEntity(response.Message);
        }


        //GET: Due Collection single
        public async Task<IActionResult> PayDue(int? id)
        {
            if (id == null) return RedirectToAction("PurchaseRecords");

            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");
            var model = await _db.Purchases.PurchaseReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);

            if (model == null) return RedirectToAction("PurchaseRecords");
            return View(model);
        }

        //vendor due collection(ajax)
        [HttpPost]
        public async Task<IActionResult> DueCollection(PurchaseDuePaySingleModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var dbResponse = await _db.PurchasePayments.DuePaySingleAsync(model, _db).ConfigureAwait(false);

            return Json(dbResponse);
        }


        //Due Receipt list
        public IActionResult DueReceipt()
        {
            return View();
        }


        //GET: Due Collection multiple
        public async Task<IActionResult> PayDueMultiple(int? id)
        {
            if (id == null) return RedirectToAction("DueReceipt");

            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");
            var model = await _db.Purchases.PurchaseReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);

            if (model == null) return RedirectToAction("DueReceipt");
            return View(model);
        }

        //vendor due collection(ajax)
        [HttpPost]
        public async Task<IActionResult> PayVendorDueMultiple(PurchaseDuePaySingleModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var dbResponse = await _db.PurchasePayments.DuePaySingleAsync(model, _db).ConfigureAwait(false);

            if (dbResponse.IsSuccess) return Ok(dbResponse);

            return BadRequest(dbResponse.Message);
        }
    }
}
