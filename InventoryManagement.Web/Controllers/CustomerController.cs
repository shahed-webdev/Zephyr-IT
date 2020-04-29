using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    public class CustomerController : Controller
    {
        //GET:// List of customer
        public IActionResult List()
        {
            return View();
        }

        //GET:// add customer
        public IActionResult Add()
        {
            return View();
        }

        //POST:// add customer
        [HttpPost]
        public IActionResult Add(Customer model)
        {
            return View(model);
        }
    }
}