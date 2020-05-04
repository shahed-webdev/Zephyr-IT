using InventoryManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    public class VendorController : Controller
    {
        private readonly IUnitOfWork _db;

        public VendorController(IUnitOfWork db)
        {
            _db = db;
        }



        // GET: Vendors/Create
        public IActionResult Create()
        {
            return View($"_Create");
        }

        // POST: Vendors/Create
        [HttpPost]
        public async Task<ActionResult> Create(VendorViewModel model)
        {
            if (!ModelState.IsValid) return View($"_Create", model);

            var vendor = _db.Vendors.AddCustom(model);
            var task = await _db.SaveChangesAsync();

            if (task != 0)
            {
                model.VendorId = vendor.VendorId;
                var result = new AjaxContent<VendorViewModel> { Status = true, Data = model };
                return Json(result);
            }

            ModelState.AddModelError("", "Unable to insert record!");
            return View($"_Create", model);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return BadRequest(HttpStatusCode.BadRequest);
            var model = _db.Vendors.FindCustom(id);

            if (model == null) return NotFound();
            //if (Request.IsAjaxRequest()) return PartialView($"_Edit", model);

            return View(model);
        }

        // POST: Vendors/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(VendorViewModel model)
        {
            //if (!ModelState.IsValid) return View(Request.IsAjaxRequest() ? "_Edit" : "Edit", model);

            _db.Vendors.UpdateCustom(model);
            var task = await _db.SaveChangesAsync();

            //if (task != 0)
            //    return Request.IsAjaxRequest() ? (ActionResult)Content("success") : RedirectToAction("Index");

            ModelState.AddModelError("", "Unable to update");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View(/*Request.IsAjaxRequest() ? "_Edit" : "Edit", model*/);
        }

        // POST: Delete/5
        public int Delete(int id)
        {
            if (!_db.Vendors.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
        }

        //GET: Details
    }
}