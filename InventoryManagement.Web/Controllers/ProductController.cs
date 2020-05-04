using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    public class ProductController : Controller
    {
        //GET: Barcode
        public IActionResult Barcode()
        {
            return View();
        }
    }
}