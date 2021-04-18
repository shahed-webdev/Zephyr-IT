using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    public class SalesmanController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        #region Sale Product

        public IActionResult SellingProduct()
        {
            return View();
        }

        #endregion
    }
}
