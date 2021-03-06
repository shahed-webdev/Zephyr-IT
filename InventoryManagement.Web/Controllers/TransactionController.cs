using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult AddAccount()
        {
            return View();
        } 
        
        public IActionResult Withdrawal()
        {
            return View();
        }

        public IActionResult Deposit()
        {
            return View();
        }
    }
}
