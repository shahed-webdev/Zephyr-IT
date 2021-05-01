using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class WarrantyController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IWarrantyCore  _warranty;
        public WarrantyController(IUnitOfWork db, IWarrantyCore warranty)
        {
            _db = db;
            _warranty = warranty;
        }


        #region Acceptance
        //find product
        public async Task<IActionResult> FindProduct(int? id)
        {
            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            return View(model);
        }

        //Find Product By Code(ajax)
        public async Task<IActionResult> FindProductByCode(string code)
        {
            var response = await _db.ProductStocks.FindSellingIdAsync(code);
            return Json(response);
        }

        //Post Acceptance(ajax)
        public IActionResult PostAcceptance(WarrantyAcceptanceModel model)
        {
            var response =  _warranty.Acceptance(model, User.Identity.Name);
            return Json(response);
        }

        //get warranty receipt
        public IActionResult AcceptanceSlip(int? id)
        {
            if (!id.HasValue) return RedirectToAction("FindProduct");

            var model =  _warranty.AcceptanceReceipt(id.GetValueOrDefault());
            return View(model.Data);
        }
        #endregion
    }
}
