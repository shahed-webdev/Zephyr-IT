using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
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

        //Add products info
        [Authorize(Roles = "admin, add-product")]
        public IActionResult AddProduct()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Product
        [HttpPost]
        public IActionResult AddProduct(ProductCatalogViewModel model)
        {
            return View();
        }

        //get product from ajax by categoryId
        public IActionResult GetProduct(int categoryId)
        {
            return View();
        }

        //delete product
        [HttpPost]
        public IActionResult DeleteProduct(int Id)
        {
            return View();
        }


        //GET: Barcode
        [Authorize(Roles = "admin, barcode")]
        public IActionResult Barcode()
        {
            return View();
        }

        //GET: Catalog list
        [Authorize(Roles = "admin, category-list")]
        public IActionResult CatalogList()
        {
            var model = _db.ProductCatalogs.ListCustom();
            return View(model);
        }

        //GET: Catalog
        [Authorize(Roles = "admin, category-list")]
        public IActionResult Catalog()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Catalog
        [Authorize(Roles = "admin, category-list")]
        [HttpPost]
        public async Task<IActionResult> Catalog(ProductCatalogViewModel model)
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            if (!ModelState.IsValid) return View(model);

            var response = await _db.ProductCatalogs.AddCustomAsync(model).ConfigureAwait(false);

            if (response.IsSuccess)
                return RedirectToAction("CatalogList");

            ModelState.AddModelError("CatalogName", response.Message);
            return View(model);
        }

        //GET: catalog Type
        public async Task<IActionResult> CatalogType()
        {
            var response = await _db.ProductCatalogTypes.ToListAsync();
            return Json(response);
        }

        //POST: catalog type
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
        [Authorize(Roles = "admin, purchase")]
        public IActionResult Purchase()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Purchase
        [Authorize(Roles = "admin, purchase")]
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

            var model = await _db.Purchases.PurchaseReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);


            return View(model);
        }

        //selling
        [Authorize(Roles = "admin, selling")]
        public IActionResult Selling()
        {
            return View();
        }

        [Authorize(Roles = "admin, selling")]
        [HttpPost]
        public async Task<IActionResult> Selling([FromBody] SellingViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);

            if (!ModelState.IsValid) UnprocessableEntity(ModelState);

            var response = await _db.Selling.AddCustomAsync(model, _db).ConfigureAwait(false);

            if (response.IsSuccess)
                return Ok(response);
            else
                return UnprocessableEntity(response);
        }

        //selling receipt
        public async Task<IActionResult> SellingReceipt(int? id)
        {
            if (id == null) return RedirectToAction("Selling");
            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            return View(model);
        }

        //call from axios
        public async Task<IActionResult> FindProductByCode(string code)
        {
            var data = await _db.ProductStocks.FindforSellAsync(code).ConfigureAwait(false);
            if (data != null) data.ProductCatalogName = _db.ProductCatalogs.CatalogNameNode(data.ProductCatalogId);

            return Json(data);
        }

        public async Task<IActionResult> FindCustomers(string prefix)
        {
            var data = await _db.Customers.SearchAsync(prefix).ConfigureAwait(false);
            return Json(data);
        }

        //selling record
        [Authorize(Roles = "admin, selling-record")]
        public IActionResult SellingRecords()
        {
            return View();
        }

        //request from datatable(ajax)
        public IActionResult SellingRecordsData(DataRequest request)
        {
            var data = _db.Selling.Records(request);
            return Json(data);
        }

        //purchase Records
        [Authorize(Roles = "admin, purchase-record")]
        public IActionResult PurchaseRecords()
        {
            return View();
        }

        //request from datatable(ajax)
        public IActionResult PurchaseRecordsData(DataRequest request)
        {
            var data = _db.Purchases.Records(request);
            return Json(data);
        }
    }
}