using InventoryManagement.Repository;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Catalog(ProductCatalogViewModel model)
        {
            if (ModelState.IsValid)
            {
               // _db.ProductCatalogs.add(model);
                //_db.SaveChanges();
            }

            return View();
        }

        //POST: Catalog type
        [HttpPost]
        public IActionResult CatalogType(ProductCatalogTypeViewModel model)
        {
            return View();
        }
    }
}