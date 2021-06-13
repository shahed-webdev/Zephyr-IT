using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class DamageController : Controller
    {
        private readonly IUnitOfWork _db;
        public DamageController(IUnitOfWork db)
        {
            _db = db;
        }


        #region Add Damage
        [Authorize(Roles = "admin, damage-find-product")]
        public IActionResult SearchProduct()
        {
            return View();
        }

        //call from ajax
        public async Task<IActionResult> FindUnsoldProduct(string code)
        {
            var data = await _db.ProductStocks.FindforDetailsAsync(code).ConfigureAwait(false);
            return Json(data);
        }
        #endregion

        [Authorize(Roles = "admin, damaged-list")]
        public IActionResult DamagedList()
        {
            return View();
        }
    }
}
