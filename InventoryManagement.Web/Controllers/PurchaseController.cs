using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        #region Purchase
        //GET: Purchase
        [Authorize(Roles = "admin, purchase")]
        public IActionResult Purchase()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");

            var defaultAccountId = _account.DefaultAccountGet();
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label", defaultAccountId);

            return View();
        }

        //POST: Purchase
        [Authorize(Roles = "admin, purchase")]
        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var response = await _db.Purchases.AddCustomAsync(model, _db).ConfigureAwait(false);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> IsPurchaseCodeExist([FromBody] List<ProductStockViewModel> stocks)
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

        //GET: check customer PhoneExist
        public IActionResult IsCustomerPhoneExist(string phone)
        {
            var data = _db.Vendors.IsPhoneExist(phone);
            return Json(data);
        }

        [Authorize(Roles = "admin, purchase")]
        public async Task<IActionResult> PurchaseReceipt(int? id)
        {
            if (id == null) return RedirectToAction("Purchase");

            var model = await _db.Purchases.PurchaseReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);

            if (model == null) return RedirectToAction("Purchase");
            return View(model);
        }
        #endregion

        #region Purchase Record

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
        #endregion

        #region Due Collection(single)

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
        #endregion

        #region Due Collection(multiple)

        //Due Receipt list
        [Authorize(Roles = "admin, due-pay-multiple")]
        public IActionResult DueReceipt()
        {
            return View();
        }

        //GET: Due Collection multiple
        [Authorize(Roles = "admin, due-pay-multiple")]
        public IActionResult PayDueMultiple(int? id)
        {
            if (id == null) return RedirectToAction("DueReceipt");

            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");

            var model = _db.PurchasePayments.GetPurchaseDuePayMultipleBill(id.GetValueOrDefault());

            if (model == null) return RedirectToAction("DueReceipt");
            return View(model);
        }

        //vendor due collection(ajax)
        [HttpPost]
        public async Task<IActionResult> PayVendorDueMultiple(PurchaseDuePayMultipleModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var response = await _db.PurchasePayments.DuePayMultipleAsync(model, _db);

            return Json(response);
        }
        #endregion

        #region Stock Report

        public async Task<IActionResult> StockReport()
        {
            var model = await _db.ProductStocks.StockReport();
            return View(model);
        }

        #endregion

        #region Bill Update
        public async Task<IActionResult> UpdatePurchaseBill(int? id)
        {
            if (!id.HasValue) return RedirectToAction("PurchaseRecords");
           
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");

            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");

            var model = await _db.Purchases.FindUpdateBillAsync(id.GetValueOrDefault(), _db);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostPurchaseBillChange(PurchaseUpdatePostModel model)
        {
            var response = await _db.Purchases.BillUpdated(model, _db, User.Identity.Name);
            return Json(response);
        }
        #endregion
    }
}
