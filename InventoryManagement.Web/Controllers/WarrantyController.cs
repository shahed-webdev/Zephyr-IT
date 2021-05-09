using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "admin, warranty-acceptance")]
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
            var response = _warranty.Acceptance(model, User.Identity.Name);
            return Json(response);
        }

        //get warranty receipt
        public IActionResult AcceptanceSlip(int? id)
        {
            if (!id.HasValue) return RedirectToAction("FindProduct");

            var model = _warranty.Receipt(id.GetValueOrDefault());
            return View(model.Data);
        }
        #endregion


        #region Warranty List
        //Get Warranty List
        [Authorize(Roles = "admin, warranty-list")]
        public IActionResult WarrantyList()
        {
            return View();
        }

        //data-table
        public IActionResult WarrantyListData(DataRequest request)
        {
            var response = _warranty.List(request);
            return Json(response);
        }
        #endregion


        #region Delivery
        //Warranty Delivery
        public IActionResult WarrantyDelivery(int? id)
        {
            if (!id.HasValue) return RedirectToAction("WarrantyList");

            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label", id.GetValueOrDefault());
            var model = _warranty.Receipt(id.GetValueOrDefault());
            return View(model.Data);
        }

        //Post Delivery
        [HttpPost]
        public IActionResult PostDelivery(WarrantyDeliveryModel model)
        {
            var response = _warranty.Delivery(model,User.Identity.Name);
            return Json(response);
        }
        #endregion
    }
}
