using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _db;
        public CustomerController(IUnitOfWork db)
        {
            _db = db;
        }

        //GET:// MobileIsAvailable
        public async Task<bool> CheckMobileIsAvailable(string mobile, int id = 0)
        {
            return await _db.Customers.IsPhoneNumberExistAsync(mobile, id);
        }

        //GET:// List of customer
        public IActionResult List()
        {
           var list = _db.Customers.ListCustom();
            return View(list);
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
            var phone = model.PhoneNumbers.FirstOrDefault().Phone;
            var checkPhone = await _db.Customers.IsPhoneNumberExistAsync(phone);

            if (!checkPhone)
            {
                _db.Customers.AddCustom(model);
                _db.SaveChanges();

                RedirectToAction("List");
            }

            return View();
        }


        //GET:// Update customer
        public IActionResult Update(int? id)
        {
            if (!id.HasValue) return BadRequest(HttpStatusCode.BadRequest);

            var model = _db.Customers.FindCustom(id.GetValueOrDefault());

            if (model == null) return NotFound();

            return View(model);
        }

        //POST:// Update customer
        [HttpPost]
        public async Task<IActionResult> Update(CustomerAddUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var phone = model.PhoneNumbers.FirstOrDefault().Phone;
            var checkPhone = await _db.Customers.IsPhoneNumberExistAsync(phone);

            if (!checkPhone)
            {
                _db.Customers.CustomUpdate(model);
                _db.SaveChanges();
                RedirectToAction("List");
            }

            return RedirectToAction("List");
        }
    }
}