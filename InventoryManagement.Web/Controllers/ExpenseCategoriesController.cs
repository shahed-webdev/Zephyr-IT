using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InventoryManagement.Data;
using InventoryManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    [Authorize(Roles = "admin, expense-category")]
    public class ExpenseCategoriesController : Controller
    {
        private readonly IUnitOfWork _db;

        public ExpenseCategoriesController(IUnitOfWork db)
        {
            _db = db;
        }

        // GET: Index
        public IActionResult Index()
        {
            return View();
        }

        //GET: Call from ajax
        public IActionResult IndexData()
        {
            var data = _db.ExpenseCategories.ToList();
            return Json(data);
        }

        // GET: Create
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Create(ExpenseCategory model)
        {
            var exist = _db.ExpenseCategories.Any(n => n.CategoryName == model.CategoryName);
            if (exist) ModelState.AddModelError("CategoryName", "Category Name already exist!");
            
            if (!ModelState.IsValid) return PartialView("_Create", model);

            _db.ExpenseCategories.Add(model);

            var task = await _db.SaveChangesAsync();

            if (task != 0) return Json(model);

            ModelState.AddModelError("", "Unable to insert record!");
            return PartialView("_Create", model);
        }

        // GET: Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return BadRequest(HttpStatusCode.BadRequest);

            var model = _db.ExpenseCategories.Find(id.GetValueOrDefault());

            if (model == null) return NotFound();

            return PartialView("_Edit", model);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Edit(ExpenseCategory model)
        {
            var exist = _db.ExpenseCategories.Any(n => (n.CategoryName == model.CategoryName) && n.ExpenseCategoryId != model.ExpenseCategoryId);
            if (exist) ModelState.AddModelError("CategoryName", "Category Name must be unique!");

            if (!ModelState.IsValid) return PartialView("_Edit", model);

            _db.ExpenseCategories.Update(model);

            var task = await _db.SaveChangesAsync();
            if (task != 0) return Json(model);

            ModelState.AddModelError("", "Unable to update");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return PartialView("_Edit", model);
        }

        // POST: Delete/5
        public int Delete(int id)
        {
            if (!_db.ExpenseCategories.RemoveCustom(id)) return -1;
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