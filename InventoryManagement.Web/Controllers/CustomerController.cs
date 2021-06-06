using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize(Roles = "admin, SalesPerson, customer-list")]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _db;
        public CustomerController(IUnitOfWork db)
        {
            _db = db;
        }

        #region Customer Add

        //GET:// Mobile Is Available(ajax)
        public async Task<bool> CheckMobileIsAvailable(string mobile, int Id = 0)
        {
            return await _db.Customers.IsPhoneNumberExistAsync(mobile, Id).ConfigureAwait(false);
        }

        //GET:// List of customer
        public IActionResult List()
        {
            return View();
        }

        //For Data-table
        public IActionResult CustomerList(DataRequest request)
        {
            var list = _db.Customers.ListDataTable(request);
            return Json(list);
        }

        //GET:// add customer
        public IActionResult Add()
        {
            return View();
        }

        //POST:// Add customer
        [HttpPost]
        public async Task<IActionResult> Add(CustomerAddUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var phone = model.PhoneNumbers.FirstOrDefault()?.Phone;
            var checkPhone = await _db.Customers.IsPhoneNumberExistAsync(phone).ConfigureAwait(false);

            if (checkPhone) return View(model);

            _db.Customers.AddCustom(model);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("List");
        }

        //GET:// Update customer
        public IActionResult Update(int? id)
        {
            if (!id.HasValue) return RedirectToAction("List");
            var model = _db.Customers.FindCustom(id.GetValueOrDefault());
            if (model == null) return NotFound();

            return View(model);
        }

        //POST:// Update customer
        [HttpPost]
        public async Task<IActionResult> Update(CustomerAddUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var phone = model.PhoneNumbers.FirstOrDefault()?.Phone;
            var checkPhone = await _db.Customers.IsPhoneNumberExistAsync(phone, model.CustomerId).ConfigureAwait(false);

            if (checkPhone) return View(model);

            _db.Customers.CustomUpdate(model);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction("List");
        }
        #endregion

        #region Customer Details

        //GET:// Details
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return RedirectToAction("List");

            var model = _db.Customers.ProfileDetails(id.GetValueOrDefault());
            if (model == null) return NotFound();

            return View(model);
        }

        //customer invoice data-table(ajax)
        [HttpPost]
        public IActionResult SellingRecordsData(DataRequest request)
        {
            var data = _db.Customers.SellingRecord(request);
            return Json(data);
        }
        #endregion
    }
}