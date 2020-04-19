using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //GET: Profile
        public IActionResult Profile()
        {
            return View();
        }

        //GET: Store Info
        public IActionResult StoreInfo()
        {
            return View();
        }
    }
}