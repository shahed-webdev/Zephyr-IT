﻿using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Controllers
{

    public class ExpensesController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IExpenseCore _expense;
        public ExpensesController(IUnitOfWork db, IExpenseCore expense)
        {
            _db = db;
            _expense = expense;
        }


        // GET: Expanses
        [Authorize(Roles = "admin, expanse")]
        public IActionResult Index()
        {
            return View(_expense.ExpenseRecords().Data);
        }

        //data-table
        //public IActionResult GetExpense(DataRequest request)
        //{
        //    var data = _expense.ExpenseRecords(request);
        //    return Json(data);
        //}

        //***General Expense***
        [Authorize(Roles = "admin, generalExpense")]
        public IActionResult GeneralExpense()
        {
            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");
            return View();
        }

        // add
        [Authorize(Roles = "admin, generalExpense")]
        [HttpPost]
        public IActionResult GeneralExpense(ExpenseAddModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label", model.ExpenseCategoryId);

            var response = _expense.AddCost(model, User.Identity.Name, User.IsInRole("admin"));

            if (response.IsSuccess)
                return RedirectToAction("GeneralExpense", new { Message = response.Message });

            return View(model);
        }

        //details
        [Authorize(Roles = "admin, expanse")]
        public IActionResult GeneralExpenseDetails(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");
           
            var model = _expense.GetCost(id.GetValueOrDefault()).Data;
            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");
           
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateGeneralExpense(ExpenseAddModel model)
        {
            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");

            var response = _expense.EditCost(model);

            if (response.IsSuccess) return RedirectToAction("Index");
            
            return View(model);
        }



        //***Transportation Cost***
        [Authorize(Roles = "admin, transportationCost")]
        public IActionResult TransportationCost()
        {
            return View();
        }


        //add
        [Authorize(Roles = "admin, transportationCost")]
        [HttpPost]
        public IActionResult PostTransportationCost(ExpenseTransportationAddModel model)
        {
            var response = _expense.AddTransportationCost(model, User.Identity.Name, User.IsInRole("admin"));
            return Json(response);
        }

        //details
        [Authorize(Roles = "admin, expanse")]
        public IActionResult TransportationCostDetails(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");

            return View(_expense.GetTransportationCostDetails(id.GetValueOrDefault()).Data);
        }

        //update
        [HttpPost]
        public IActionResult UpdateTransportationCost(ExpenseTransportationDetailsModel model)
        {
            var response = _expense.EditTransportationCost(model);
            return Json(response);
        }


        //approve
        [HttpPost]
        public IActionResult ApproveTransportationCost(int id)
        {
            var response = _expense.ApprovedTransportationCost(id);
            return Json(response);
        }


        //**Fixed Cost***
        [Authorize(Roles = "admin, fixedCost")]
        public IActionResult FixedCost()
        {
            var model = _expense.FixedCostRecords();
            return View(model.Data);
        }

        //add
        [Authorize(Roles = "admin, fixedCost")]
        [HttpPost]
        public IActionResult PostFixedCost(ExpenseFixedAddModel model)
        {
            var response = _expense.AddFixedCost(model);
            return Json(response);
        }

        //delete
        [Authorize(Roles = "admin, fixedCost")]
        public IActionResult DeleteFixedCost(int id)
        {
            var response = _expense.DeleteFixedCost(id);
            return Json(response);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}