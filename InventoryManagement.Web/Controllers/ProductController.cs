using InventoryManagement.Repository;
using Microsoft.AspNetCore.Mvc;
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

        //GET: Catalog
        public IActionResult Catalog()
        {
            return View();
        }

        //POST: Catalog
        [HttpPost]
        public async Task<IActionResult> Catalog(ProductCatalogViewModel model)
        {
            if (ModelState.IsValid)
            {

                var response = await _db.ProductCatalogs.AddCustomAsync(model).ConfigureAwait(false);

                return response.IsSuccess ? Json(response.Data) : Json(response.Message);
            }

            return View();
        }

        //POST: Catalog type
        [HttpPost]
        public async Task<IActionResult> CatalogType(ProductCatalogTypeViewModel model)
        {
            var response = await _db.ProductCatalogTypes.AddCustomAsync(model).ConfigureAwait(false);

            return response.IsSuccess ? Json(response.Data) : Json(response.Message);
        }
    }
}