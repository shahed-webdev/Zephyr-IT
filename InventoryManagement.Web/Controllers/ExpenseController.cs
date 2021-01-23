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

        public IActionResult IndexData()
        {
            var data = _db.Expenses.ToListCustom();
            return Json(data);
        }

        //General Expense
        [Authorize(Roles = "admin, generalExpense")]
        public IActionResult GeneralExpense()
        {
            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");
            return View();
        }


        // POST:General Expanses
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


        //Transportation Cost
        [Authorize(Roles = "admin, transportationCost")]
        public IActionResult TransportationCost()
        {
            return View();
        }


        //POST: Transportation Cost
        [Authorize(Roles = "admin, transportationCost")]
        [HttpPost]
        public IActionResult PostTransportationCost(ExpenseTransportationAddModel model)
        {
            var response = _expense.AddTransportationCost(model, User.Identity.Name, User.IsInRole("admin"));
            return Json(response);
        }



        // POST: Delete/5
        //public int Delete(int id)
        //{
        //    _db.Expenses.RemoveCustom(id);
        //    return _db.SaveChanges();
        //}


        //General Expense
        [Authorize(Roles = "admin, fixedCost")]
        public IActionResult FixedCost()
        {
            return View();
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