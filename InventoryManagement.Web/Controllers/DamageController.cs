﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    public class DamageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
