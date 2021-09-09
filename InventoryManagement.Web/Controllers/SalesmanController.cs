using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Controllers
{
    [Authorize(Roles = "SalesPerson")]
    public class SalesmanController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IAccountCore _account;
        public SalesmanController(IAccountCore account, IUnitOfWork db)
        {
            _account = account;
            _db = db;
        }

        public IActionResult Dashboard()
        {
            ViewBag.Balance = _db.Registrations.Balance(User.Identity.Name);
            ViewBag.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
           
            return View();
        }

        #region Sale Product
        public IActionResult SellingProduct()
        {
            var defaultAccountId = _account.DefaultAccountGet();
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label", defaultAccountId);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Selling(SellingViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);

            if (!ModelState.IsValid) UnprocessableEntity(ModelState);

            var response = await _db.Selling.AddCustomAsync(model, _db).ConfigureAwait(false);

            return Json(response);
        }
        #endregion

        #region Product Stock
        public IActionResult ProductStock()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        #endregion

        #region Reports
        //selling report
        public IActionResult SellingRecords()
        {
            ViewBag.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            return View();
        }

        //Cash Collection records
        public IActionResult CashCollectionReport()
        {
            ViewBag.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            return View();
        }
        #endregion
    }
}
