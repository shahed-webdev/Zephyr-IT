using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;

        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }

        //GET: Barcode
        public IActionResult Barcode()
        {
            return View();
        }

        //GET: Catalog list
        public IActionResult CatalogList()
        {
            var model = _db.ProductCatalogs.ListCustom();
            return View(model);
        }

        //GET: Catalog
        public IActionResult Catalog()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Catalog
        [HttpPost]
        public async Task<IActionResult> Catalog(ProductCatalogViewModel model)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            var response = await _db.ProductCatalogs.AddCustomAsync(model).ConfigureAwait(false);

            if (response.IsSuccess)
                return RedirectToAction("CatalogList");
            else
                return UnprocessableEntity(response.Message);
        }

        //GET: Catalog Type
        public async Task<IActionResult> CatalogType()
        {
            var response = await _db.ProductCatalogTypes.ToListAsync();
            return Json(response);
        }

        //POST: Catalog type
        [HttpPost]
        public async Task<IActionResult> CatalogType([FromBody] ProductCatalogTypeViewModel model)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            var response = await _db.ProductCatalogTypes.AddCustomAsync(model).ConfigureAwait(false);

            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return UnprocessableEntity(response.Message);
        }

        //GET: find vendor from ajax autocomplete
        public async Task<IActionResult> FindVendor(string prefix)
        {
            var data = await _db.Vendors.SearchAsync(prefix).ConfigureAwait(false);
            return Json(data);
        }

        //GET: Purchase
        public IActionResult Purchase()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Purchase
        [HttpPost]
        public async Task<IActionResult> Purchase([FromBody] PurchaseViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            
            if (!ModelState.IsValid) UnprocessableEntity(ModelState);

            var response = await _db.Purchases.AddCustomAsync(model, _db).ConfigureAwait(false);

            if (response.IsSuccess)
                return Ok(response);
            else
                return UnprocessableEntity(response);
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseCodeIsExist([FromBody] List<ProductStockViewModel> stocks)
        {
            var existList = await _db.ProductStocks.IsExistListAsync(stocks).ConfigureAwait(false);
            return Json(existList);
        }

        public async Task<IActionResult> PurchaseReceipt(int? id)
        {
            if (id == null) return RedirectToAction("Purchase");

            var model = await _db.Purchases.PurchaseReceiptAsync(id.GetValueOrDefault(),_db).ConfigureAwait(false);


            return View(model);
        }

        //selling
        public IActionResult Selling()
        {
            return View();
        }
    }
}