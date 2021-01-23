using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

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
            return View();
        }

        //data-table
        public IActionResult GetExpense()
        {
            var data = _db.Expenses.ToListCustom();
            return Json(data);
        }

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
        public async Task<IActionResult> GeneralExpense(ExpenseAddModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);

            if (!ModelState.IsValid) return View(model);

            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label", model.ExpenseCategoryId);

            var voucherNo = _db.Institutions.GetVoucherCountdown() + 1;

            _db.Expenses.AddCustom(model, voucherNo, User.IsInRole("admin"));
            _db.Institutions.IncreaseVoucherCount();

            var task = await _db.SaveChangesAsync();

            if (task != 0)
                return RedirectToAction("GeneralExpense", new { Message = "Expense Added Successfully!" });

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


        public IActionResult TransportationCostDetails(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index");

            return View();
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