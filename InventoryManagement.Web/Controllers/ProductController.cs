using InventoryManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
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


        //GET: Purchase
        public IActionResult Purchase()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Purchase
        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseViewModel model)
        {
            if (!ModelState.IsValid) return View(model);


            return View(model);
        }
    }
}