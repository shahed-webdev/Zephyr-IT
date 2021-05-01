using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Repository;

namespace InventoryManagement.Web.Controllers
{
    public class WarrantyController : Controller
    {
        private readonly IUnitOfWork _db;
        public WarrantyController(IUnitOfWork db)
        {
            _db = db;
        }


        #region Acceptance
        //find product
        public IActionResult FindProduct()
        {
            return View();
        }

        //Find Product By Code
        public async Task<IActionResult> FindProductByCode(string code)
        {
            var response = await _db.ProductStocks.FindSellingIdAsync(code);
            return Json(response);
        }

        public async Task<IActionResult> AcceptanceSlip(int? id)
        {
            if (!id.HasValue) return RedirectToAction("FindProduct");

            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
        
            return View(model);
        }
        #endregion
    }
}
