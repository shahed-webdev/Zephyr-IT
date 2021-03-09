using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
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
            var response = _accountCore.List();
            return View(response);
        }

        //add
        [HttpPost]
        public IActionResult AddAccount(AccountCrudModel model)
        {
            var response = _accountCore.Add(model);
            return Json(response);
        }

        //update
        [HttpPost]
        public IActionResult UpdateAccount(AccountCrudModel model)
        {
            var response = _accountCore.Edit(model);
            return Json(response);
        }

        //delete
        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            var response = _accountCore.Delete(id);
            return Json(response);
        }

        //***Withdrawal****
        public IActionResult Withdrawal(int? id)
        {
            if (!id.HasValue) return RedirectToAction("AddAccount");
            return View(_accountCore.Get(id.GetValueOrDefault()).Data);
        }

        //Add
        public IActionResult AddWithdrawal(AccountWithdrawCrudModel model)
        {
            var response = _accountCore.Withdraw(model);
            return Json(response);
        }
        
        //delete
        [HttpPost]
        public IActionResult DeleteWithdrawal(int id)
        {
            var response = _accountCore.WithdrawDelete(id);
            return Json(response);
        }



        //***Deposit***
        public IActionResult Deposit(int? id)
        {
            if (!id.HasValue) return RedirectToAction("AddAccount");
            return View(_accountCore.Get(id.GetValueOrDefault()).Data);
        }

        //Add
        public IActionResult AddDeposit(AccountDepositCrudModel model)
        {
            var response = _accountCore.Deposit(model);
            return Json(response);
        }

        //delete
        [HttpPost]
        public IActionResult DeleteDeposit(int id)
        {
            var response = _accountCore.DepositDelete(id);
            return Json(response);
        }
    }
}
