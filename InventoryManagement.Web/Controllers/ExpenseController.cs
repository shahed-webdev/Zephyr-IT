using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize(Roles = "admin, expanse")]
    public class ExpensesController : Controller
    {
        private readonly IUnitOfWork _db;

        public ExpensesController(IUnitOfWork db)
        {
            _db = db;
        }

        // GET: Expanses
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexData()
        {
            var data = _db.Expenses.ToListCustom();
            return Json(data);
        }

        // GET: Expanses/Create
        public IActionResult Create()
        {
            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label");
            return PartialView("_Create");
        }

        // POST: Expanses/Create
        [HttpPost]
        public async Task<IActionResult> Create(ExpenseViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            if (!ModelState.IsValid) return View($"_Create", model);

            ViewBag.ExpenseCategoryId = new SelectList(_db.ExpenseCategories.ddl(), "value", "label", model.ExpenseCategoryId);


            _db.Expenses.AddCustom(model);

            var task = await _db.SaveChangesAsync();

            if (task != 0) return Content("success");

            ModelState.AddModelError("", "Unable to insert record!");
            return PartialView("_Create", model);
        }

        // POST: Delete/5
        public int Delete(int id)
        {
            _db.Expenses.RemoveCustom(id);
            return _db.SaveChanges();
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