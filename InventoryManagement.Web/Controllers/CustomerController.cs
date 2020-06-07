using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _db;
        public CustomerController(IUnitOfWork db)
        {
            _db = db;
        }

        //GET:// MobileIsAvailable
        public async Task<bool> CheckMobileIsAvailable(string mobile, int Id = 0)
        {
            return await _db.Customers.IsPhoneNumberExistAsync(mobile, Id).ConfigureAwait(false);
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
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //POST:// Add customer
        [HttpPost]
        public async Task<IActionResult> Add(CustomerAddUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var phone = model.PhoneNumbers.FirstOrDefault().Phone;
            var checkPhone = await _db.Customers.IsPhoneNumberExistAsync(phone).ConfigureAwait(false);

            if (checkPhone) return View(model);

            _db.Customers.AddCustom(model);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            //if (returnUrl != string.Empty)
            //{
            //    // _db.Customers.;
            //}

            return RedirectToAction("List");

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
            var phone = model.PhoneNumbers.FirstOrDefault()?.Phone;
            var checkPhone = await _db.Customers.IsPhoneNumberExistAsync(phone, model.CustomerId).ConfigureAwait(false);


            if (checkPhone == false)
            {
                _db.Customers.CustomUpdate(model);
                await _db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("List");
            }

            return View(model);
        }
    }
}