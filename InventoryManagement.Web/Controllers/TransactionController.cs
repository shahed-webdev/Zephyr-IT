using InventoryManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    public class TransactionController : Controller
    {

        //*** Account****
        public IActionResult AddAccount()
        {
            return View();
        }

        //Post New Account
        [HttpPost]
        public IActionResult PostNewAccount(AccountCrudModel model)
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
