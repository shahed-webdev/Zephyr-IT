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
        public PurchaseController(IUnitOfWork db)
        {
            _db = db;
        }

        //GET: Purchase
        [Authorize(Roles = "admin, purchase")]
        public IActionResult Purchase()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
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

            if (response.IsSuccess)
            {
                _db.Vendors.UpdatePaidDue(model.VendorId);
                await _db.SaveChangesAsync();
                return Ok(response);
            }
            else
                return UnprocessableEntity(response);
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


        public async Task<IActionResult> PayDue(int? id)
        {
            if (id == null) return RedirectToAction("PurchaseRecords");

            var model = await _db.Purchases.PurchaseReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);

            if (model == null) return RedirectToAction("PurchaseRecords");
            return View(model);
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
