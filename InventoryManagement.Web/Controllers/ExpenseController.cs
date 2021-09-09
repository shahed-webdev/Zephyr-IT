using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace InventoryManagement.Web.Controllers
{

    public class ExpensesController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IExpenseCore _expense;
        private readonly IAccountCore _account;
        public ExpensesController(IUnitOfWork db, IExpenseCore expense, IAccountCore account)
        {
            _db = db;
            _expense = expense;
            _account = account;
        }


        // GET: Expanses
        [Authorize(Roles = "admin, expanse")]
        public IActionResult Index()
        {
            return View();
        }

        //data-table
        public IActionResult GetExpense(DataRequest request)
        {
            var response = _expense.ExpenseRecords(request);
            return Json(response.Data);
        }

        #region General Expense
        //***General Expense***
        [Authorize(Roles = "admin,SalesPerson, generalExpense")]
        public IActionResult GeneralExpense()
        {
            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");

            var defaultAccountId = _account.DefaultAccountGet();
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label", defaultAccountId);
            
            return View();
        }

        // add
        [Authorize(Roles = "admin,SalesPerson, generalExpense")]
        [HttpPost]
        public IActionResult GeneralExpense(ExpenseAddModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label", model.ExpenseCategoryId);

            var response = _expense.AddCost(model, User.Identity.Name, User.IsInRole("admin"));

            if (response.IsSuccess)
                return RedirectToAction("GeneralExpense", new { response.Message });

            return View(model);
        }

        //details
        [Authorize(Roles = "admin, expanse")]
        public IActionResult GeneralExpenseDetails(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");

            var model = _expense.GetCost(id.GetValueOrDefault()).Data;

            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");

            var defaultAccountId = _account.DefaultAccountGet();
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label", defaultAccountId);

            return View(model);
        }

        //update
        [HttpPost]
        public IActionResult UpdateGeneralExpense(ExpenseAddModel model)
        {
            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");

            var response = _expense.EditCost(model);

            if (response.IsSuccess) return RedirectToAction("Index");

            return Json(model);
        }

        //approve
        [HttpPost]
        public IActionResult ApproveGeneralExpense(int id, int? accountId)
        {
            var response = _expense.ApprovedCost(id, accountId);
            return Json(response);
        }

        //delete
        [HttpPost]
        public IActionResult DeleteGeneralExpense(int id)
        {
            var response = _expense.DeleteCost(id);
            return Json(response);
        }
        #endregion

        #region Transportation Cost
        //***Transportation Cost***
        [Authorize(Roles = "admin,SalesPerson, transportationCost")]
        public IActionResult TransportationCost()
        {
            var defaultAccountId = _account.DefaultAccountGet();
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label", defaultAccountId);

            return View();
        }


        //add
        [Authorize(Roles = "admin,SalesPerson, transportationCost")]
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

            var defaultAccountId = _account.DefaultAccountGet();
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label", defaultAccountId);

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
        public IActionResult ApproveTransportationCost(int id, int? accountId)
        {
            var response = _expense.ApprovedTransportationCost(id, accountId);
            return Json(response);
        }

        //delete
        [HttpPost]
        public IActionResult DeleteTransportationCost(int id)
        {
            var response = _expense.DeleteTransportationCost(id);
            return Json(response);
        }
        #endregion

        #region Fixed Cost
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
        #endregion

        //Report
        [Authorize(Roles = "admin, expenseReport")]
        public IActionResult ExpenseReport(DateTime? from, DateTime? to)
        {
            return View(_expense.CategoryWistSummaryDateToDate(from, to).Data);
        }


        [Authorize(Roles = "admin, expenseReport")]
        public IActionResult ExpenseCategoryDetails(string category, DateTime? from, DateTime? to)
        {
            if (string.IsNullOrEmpty(category))
                return RedirectToAction("ExpenseReport");

            return View(_expense.CategoryWistDetailsDateToDate(category, from, to).Data);
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