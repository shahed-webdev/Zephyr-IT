using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        //set default account
        [Authorize(Roles = "admin, account")]
        [HttpPost]
        public IActionResult SetDefaultAccount(int? accountId)
        {
            if(accountId == null) return Json("No Account Selected");

            _accountCore.DefaultAccountSet(accountId.GetValueOrDefault());
            return Json("Default Account Set Successfully");
        }


        //*** Account****
        [Authorize(Roles = "admin, account")]
        public IActionResult AddAccount()
        {
            var response = _accountCore.List();
            
            var defaultAccountId = _accountCore.DefaultAccountGet();
            ViewBag.DefaultAccountId = defaultAccountId;
            ViewBag.Account = new SelectList(_accountCore.DdlList(), "value", "label", defaultAccountId);
           
            return View(response);
        }

        //add
        [Authorize(Roles = "admin, account")]
        [HttpPost]
        public IActionResult AddAccount(AccountCrudModel model)
        {
            var response = _accountCore.Add(model);
            return Json(response);
        }

        //update
        [Authorize(Roles = "admin, account")]
        [HttpPost]
        public IActionResult UpdateAccount(AccountCrudModel model)
        {
            var response = _accountCore.Edit(model);
            return Json(response);
        }

        //delete
        [Authorize(Roles = "admin, account")]
        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            var response = _accountCore.Delete(id);
            return Json(response);
        }



        //***Withdrawal****
        [Authorize(Roles = "admin, account")]
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

        //get withdraw data-table
        public IActionResult GetWithdrawalData(DataRequest request)
        {
            var response = _accountCore.WithdrawList(request);
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
        [Authorize(Roles = "admin, account")]
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

        //get deposit data-table
        public IActionResult GetDepositData(DataRequest request)
        {
            var response = _accountCore.DepositList(request);
            return Json(response);
        }

        //delete
        [HttpPost]
        public IActionResult DeleteDeposit(int id)
        {
            var response = _accountCore.DepositDelete(id);
            return Json(response);
        }


        // Transfer To Default Account
        [HttpPost]
        public IActionResult TransferToDefaultAccount(int accountId, decimal amount)
        {
            var response = _accountCore.TransferToDefault(accountId, amount);
            return Json(response);
        }


        /***CAPITAL***/
       [Authorize(Roles = "admin, capital")]
        public IActionResult Capital()
        {
            ViewBag.Capital = _accountCore.CapitalGet();
            return View();
        }


        //post capital
        [Authorize(Roles = "admin, capital")]
        [HttpPost]
        public IActionResult Capital(decimal amount)
        {
            _accountCore.CapitalSet(amount);
            ViewBag.Capital = _accountCore.CapitalGet();

            return View();
        }
    }
}
