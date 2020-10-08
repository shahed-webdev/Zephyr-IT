using System;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class SellingController : Controller
    {
        private readonly IUnitOfWork _db;
        public SellingController(IUnitOfWork db)
        {
            _db = db;
        }

        //selling
        [Authorize(Roles = "admin, selling")]
        public IActionResult Selling()
        {
            return View();
        }

        [Authorize(Roles = "admin, selling")]
        [HttpPost]
        public async Task<IActionResult> Selling([FromBody] SellingViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);

            if (!ModelState.IsValid) UnprocessableEntity(ModelState);

            var response = await _db.Selling.AddCustomAsync(model, _db).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return UnprocessableEntity(response);
            }
        }

        //Selling receipt
        public async Task<IActionResult> SellingReceipt(int? id)
        {
            if (id == null) return RedirectToAction("Selling");

            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            if (model == null) return RedirectToAction("Selling");

            return View(model);
        }

        //call from axios
        public async Task<IActionResult> FindProductByCode(string code)
        {
            var data = await _db.ProductStocks.FindforSellAsync(code).ConfigureAwait(false);
            return Json(data);
        }

        public async Task<IActionResult> FindCustomers(string prefix)
        {
            var data = await _db.Customers.SearchAsync(prefix).ConfigureAwait(false);
            return Json(data);
        }

        //selling record
        [Authorize(Roles = "admin, selling-record")]
        public IActionResult SellingRecords()
        {
            return View();
        }

        //request from data-table(ajax)
        public IActionResult SellingRecordsData(DataRequest request)
        {
            var data = _db.Selling.Records(request);
            return Json(data);
        }


        //GET: Due Collection
        public async Task<IActionResult> DueCollection(int? id)
        {
            if (id == null) return RedirectToAction("List", "Customer");

            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            if (model == null) return RedirectToAction("List", "Customer");

            return View(model);
        }

        //customer due collection(ajax)
        [HttpPost]
        public async Task<IActionResult> DueCollection([FromBody] SellingDuePaySingleModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var dbResponse = await _db.SellingPayments.DuePaySingleAsync(model, _db).ConfigureAwait(false);

            if (dbResponse.IsSuccess) return Ok(dbResponse);

            return BadRequest(dbResponse.Message);
        }


        [HttpPost]
        public async void DueCollectionMultiple(SellingDuePayMultipleModel model)
        {
            if (model.PaidAmount <= 0) BadRequest("");

            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);

            var dbResponse = await _db.SellingPayments.DuePayMultipleAsync(model, _db).ConfigureAwait(false);

            if (dbResponse.IsSuccess)
            {
                Ok();
            }
            else
            {
                BadRequest(dbResponse.Message);
            }
        }


        //Post: Change Bill
        [Authorize(Roles = "admin, bill-change")]
        public IActionResult BillList()
        {
            return View();
        }

        public async Task<IActionResult> BillChange(int? id)
        {
            if (id == null) return RedirectToAction("BillList");
            var data = await _db.Selling.FindUpdateBillAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);

            if (data == null) return RedirectToAction("BillList");
            return View(data);
        }

        [Authorize(Roles = "admin, bill-change")]
        [HttpPost]
        public async Task<IActionResult> BillChange([FromBody] SellingUpdatePostModel model)
        {
            var regId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            model.UpdateRegistrationId = regId;

            var dbResponse = await _db.Selling.BillUpdated(model, _db);

            if (dbResponse.IsSuccess) return Ok(model.SellingId);

            return UnprocessableEntity(dbResponse.Message);
        }


        //delete Bill
        [Authorize(Roles = "admin, bill-change")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var dbResponse = await _db.Selling.DeleteBillAsync(id, _db).ConfigureAwait(false);
           
            if (dbResponse.IsSuccess) return Ok();

            return UnprocessableEntity(dbResponse.Message);
        }


        //Sales report
        public IActionResult SalesReport()
        {
            return View();
        }

        //request from data-table(ajax)
        public IActionResult SalesReportData(DataRequest request)
        {
            var data = _db.Selling.Records(request);
            return Json(data);
        }

        //GET:// Get Amount ByDate(ajax)
        public IActionResult GetSaleAmountByDate(DateTime? fromDate, DateTime? toDate)
        {
            var model = _db.Selling.ProductSoldAmountDateWise(fromDate, toDate);
            return Json(model);
        }


        //Product Sold Report
        public IActionResult ProductSoldReport()
        {
            return View();
        }
        
        //request from data-table(ajax)
        public IActionResult ProductSoldReportData(DataRequest request)
        {
            var data = _db.Selling.Records(request);
            return Json(data);
        }

        //GET:// Get Sold Amount ByDate(ajax)
        public IActionResult GetAmountByDate(DateTime? fromDate, DateTime? toDate)
        {
            var model = _db.Selling.ProductSoldAmountDateWise(fromDate, toDate);
            return Json(model);
        }


        //Cash Collection
        public IActionResult CashCollection()
        {
            return View();
        }

        //request from data-table(ajax)
        public IActionResult CashCollectionData(DataRequest request)
        {
            var data = _db.SellingPayments.Records(request);
            return Json(data);
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
