using InventoryManagement.BusinessLogin;
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
        private readonly IAccountCore _accountCore;
        public TransactionController(IAccountCore accountCore)
        {
            _accountCore = accountCore;
        }


        //*** Account****
        public IActionResult AddAccount()
        {
           // var response = _accountCore.List();
            return View();
        }

        //Post New Account
        [HttpPost]
        public IActionResult PostNewAccount(AccountCrudModel model)
        {
            var response = _accountCore.Add(model);
            return Json(response);
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
