using InventoryManagement.BusinessLogin;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class SellingController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IAccountCore _account;
        public SellingController(IUnitOfWork db, IAccountCore account)
        {
            _db = db;
            _account = account;
        }

        #region Selling
        //selling
        [Authorize(Roles = "admin, selling")]
        public IActionResult Selling()
        {
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");
            return View();
        }

        [Authorize(Roles = "admin, selling")]
        [HttpPost]
        public async Task<IActionResult> Selling(SellingViewModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);

            if (!ModelState.IsValid) UnprocessableEntity(ModelState);

            var response = await _db.Selling.AddCustomAsync(model, _db).ConfigureAwait(false);

            return Json(response);
        }

        //Selling receipt
        public async Task<IActionResult> SellingReceipt(int? id)
        {
            if (id == null) return RedirectToAction("Selling");

            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            if (model == null) return RedirectToAction("Selling");

            return View(model);
        }
        #endregion

        #region Bill expense
        //get
        public IActionResult GetBillExpense(int id)
        {
            var response = _db.Selling.ExpenseList(id);
            return Json(response);
        }

        //post bill expense
        [HttpPost]
        public async Task<IActionResult> AddBillExpense(SellingExpenseAddModel model)
        {
            var response = await _db.Selling.ExpenseAdd(model);
            return Json(response);
        }

        //delete bill expense
        [HttpPost]
        public async Task<IActionResult> DeleteBillExpense(int id)
        {
            var response = await _db.Selling.ExpenseDelete(id);
            return Json(response);
        }
        #endregion

        //call from ajax
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

        //selling invoice
        [Authorize(Roles = "admin, selling-record")]
        public IActionResult SellingRecords()
        {
            return View();
        }

        //selling invoice data-table(ajax)
        [HttpPost]
        public IActionResult SellingRecordsData(DataRequest request)
        {
            var data = _db.Selling.Records(request);
            return Json(data);
        }

        #region Due Invoice
        //due invoice
        [Authorize(Roles = "admin, due-invoice")]
        public IActionResult DueInvoice()
        {
            return View();
        }

        //due invoice data-table(ajax)
        [HttpPost]
        public IActionResult DueInvoiceData(DataRequest request)
        {
            var data = _db.Selling.DueRecords(request);
            return Json(data);
        }

        //GET: Due Collection
        public async Task<IActionResult> DueCollection(int? id)
        {
            if (id == null) return RedirectToAction("List", "Customer");

            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");

            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            if (model == null) return RedirectToAction("List", "Customer");

            return View(model);
        }

        //Customer due collection(ajax)
        [HttpPost]
        public async Task<IActionResult> DueCollection(SellingDuePaySingleModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var dbResponse = await _db.SellingPayments.DuePaySingleAsync(model, _db).ConfigureAwait(false);

            return Json(dbResponse);
        }


        [HttpPost]
        public async Task<IActionResult> DueCollectionMultiple(SellingDuePayMultipleModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var dbResponse = await _db.SellingPayments.DuePayMultipleAsync(model, _db).ConfigureAwait(false);

            return Json(dbResponse);
        }
        #endregion Due Invoice


        #region Update Bill
        //Post: Change Bill
        [Authorize(Roles = "admin, bill-change")]
        public IActionResult BillList()
        {
            return View();
        }

        [Authorize(Roles = "admin, bill-change")]
        public async Task<IActionResult> BillChange(int? id)
        {
            if (id == null) return RedirectToAction("BillList");
            ViewBag.Account = new SelectList(_account.DdlList(), "value", "label");

            var data = await _db.Selling.FindUpdateBillAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);

            if (data == null) return RedirectToAction("BillList");
            return View(data);
        }

        [Authorize(Roles = "admin, bill-change")]
        [HttpPost]
        public async Task<IActionResult> BillChange(SellingUpdatePostModel model)
        {
            var regId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            model.UpdateRegistrationId = regId;

            var dbResponse = await _db.Selling.BillUpdated(model, _db);
            return Json(dbResponse);
        }


        //delete Bill
        [Authorize(Roles = "admin, bill-change")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var dbResponse = await _db.Selling.DeleteBillAsync(id, _db).ConfigureAwait(false);

            if (dbResponse.IsSuccess) return Ok();

            return UnprocessableEntity(dbResponse.Message);
        }
        #endregion Update bill


        #region Sales Report from Dashboard
        //Sales report
        [Authorize(Roles = "admin")]
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
            var model = _db.Selling.SaleAmountDateWise(fromDate, toDate);
            return Json(model);
        }
        #endregion Sales report


        #region Product Sold Report
        //Product Sold Report
        [Authorize(Roles = "admin")]
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
        #endregion Product Sold Report


        #region Cash Collection
        //Cash Collection
        [Authorize(Roles = "admin")]
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

        //GET:// Get Collection Amount ByDate(ajax)
        public IActionResult GetCollectionByDate(DateTime? fromDate, DateTime? toDate)
        {
            var model = _db.SellingPayments.CollectionAmountDateWise(fromDate, toDate);
            return Json(model);
        }
        #endregion Cash Collection


        #region Bill Wise Profite 
        //Bill Profit
        [Authorize(Roles = "admin, bii-profite")]
        public IActionResult BillProfite()
        {
            return View();
        }

        //request from data-table(ajax)
        public IActionResult BillProfiteData(DataRequest request)
        {
            var data = _db.Selling.BillWiseProfits(request);
            return Json(data);
        }

        //GET:// Get Amount ByDate(ajax)
        [HttpPost]
        public IActionResult BillProfitByDate(DateTime? fromDate, DateTime? toDate)
        {
            var model = _db.Selling.SellingBillProfitSummaryDateWise(fromDate, toDate);
            return Json(model);
        }
        #endregion Sales report
    }
}
