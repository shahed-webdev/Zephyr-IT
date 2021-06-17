using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
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

        #region Add Product
        //Add products info
        [Authorize(Roles = "admin, add-product")]
        public IActionResult AddProduct(int? id)
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label", id.GetValueOrDefault());
            return View();
        }

        //POST: Product
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductShowViewModel model)
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");

            if (!ModelState.IsValid) return View(model);

            var isExist = await _db.Products.IsExistAsync(model.ProductName, model.ProductCatalogId).ConfigureAwait(false);

            if (!isExist)
            {
                _db.Products.AddCustom(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("AddProduct");
            }

            ModelState.AddModelError("ProductName", "Product Name Already Exist");
            return View(model);
        }

        //get product info from ajax by productId
        public async Task<IActionResult> GetProductInfo(int productId)
        {
            var productList = await _db.Products.FindByIdAsync(productId);
            return Json(productList);
        }

        //delete product
        public int DeleteProduct(int id)
        {
            if (!_db.Products.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
        }

        //get product from ajax by categoryId (data-table)
        public IActionResult GetProductByCategory(DataRequest request)
        {
            var productList = _db.Products.FindDataTable(request);
            return Json(productList);
        }

        //get product by categoryId in dropdown
        public async Task<IActionResult> GetProductByCategoryDropDown(int categoryId)
        {
            var productList = await _db.Products.FindByCategoryAsync(categoryId);
            return Json(productList);
        }

        //Stock Details
        public IActionResult StockDetails(int? id)
        {
            if (id == null) return RedirectToAction("AddProduct");
            var model = _db.Products.ProductWithCodes(id.GetValueOrDefault());

            return View(model.Data);
        }


        //stock out product details
        [HttpPost]
        public IActionResult StockOutProductBill(int productId)
        {
            var response = _db.Products.GetLastPurchaseId(productId);
            return Json(response);
        }
        #endregion

        //GET: Barcode
        [Authorize(Roles = "admin, barcode")]
        public IActionResult Barcode()
        {
            return View();
        }

        #region Product Category

        //GET: Catalog list
        [Authorize(Roles = "admin, category-list")]
        public IActionResult CatalogList()
        {
            var model = _db.ProductCatalogs.ListCustom();
            return View(model);
        }

        //Delete Catalog
        public bool DeleteCatalog(int id)
        {
            try
            {
                _db.ProductCatalogs.DeleteCustom(id);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

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
        [Authorize(Roles = "admin, category-list")]
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

            return UnprocessableEntity(response.Message);
        }


        //GET: catalog update
        [Authorize(Roles = "admin, category-list")]
        public IActionResult CatalogUpdate(int? id)
        {
            if (id == null) return RedirectToAction("CatalogList");

            var model = _db.ProductCatalogs.FindForUpdate(id.GetValueOrDefault());

            if (model == null) return NotFound();

            return View(model);
        }

        //POST: catalog update
        [HttpPost]
        public IActionResult CatalogUpdate(ProductCatalogUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _db.ProductCatalogs.UpdateCustom(model);
                _db.SaveChanges();
                return RedirectToAction("CatalogList");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("CatalogName", e.Message);
                return View(model);
            }
        }
        #endregion

        #region Find Product
        //GET: Find Product
        [Authorize(Roles = "admin, find-product, SalesPerson")]
        public IActionResult FindProduct()
        {
            return View();
        }

        //call from axios
        public async Task<IActionResult> FindProductDetailsByCode(string code)
        {
            var data = await _db.ProductStocks.FindforDetailsAsync(code).ConfigureAwait(false);
            return Json(data);
        }

        //product log
        public async Task<IActionResult> GetProductLog(string code)
        {
            var data = await _db.ProductLog.FindLogByCodeAsync(code);
            return Json(data);
        }
        #endregion
    }
}