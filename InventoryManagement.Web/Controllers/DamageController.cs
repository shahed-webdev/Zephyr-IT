using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class DamageController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IProductDamagedCore _damaged;
        public DamageController(IUnitOfWork db, IProductDamagedCore damaged)
        {
            _db = db;
            _damaged = damaged;
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
            var data = await _db.ProductStocks.FindforSellAsync(code);
            return Json(data);
        }

        [HttpPost]
        public IActionResult PostDamage(ProductDamagedAddModel model)
        {
            var response = _damaged.Add(model, User.Identity.Name);
            return Json(response);
        }
        #endregion


        #region Damage list
        [Authorize(Roles = "admin, damaged-list")]
        public IActionResult DamagedList()
        {
            return View();
        }
        
        //restock
        [HttpPost]
        public IActionResult ReStockProduct(int id)
        {
            var response = _damaged.Delete(id,User.Identity.Name);
            return Json(response);
        }

        //data-table
        public IActionResult DamageListData(DataRequest request)
        {
            var response = _damaged.List(request);
            return Json(response);
        }
        #endregion
    }
}
