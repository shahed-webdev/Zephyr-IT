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
            if (data != null) data.ProductCatalogName = _db.ProductCatalogs.CatalogNameNode(data.ProductCatalogId);

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
        public async Task<ActionResult> DueCollection(int? id)
        {
            if (id == null) return RedirectToAction($"Record");
            var model = await _db.Selling.SellingReceiptAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            if (model == null) return RedirectToAction($"Record");
            return View(model);
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
        [HttpPost]
        public async void DueCollectionSingle(SellingDuePaySingleModel model)
        {
            model.RegistrationId = _db.Registrations.GetRegID_ByUserName(User.Identity.Name);
            var dbResponse = await _db.SellingPayments.DuePaySingleAsync(model, _db).ConfigureAwait(false);

            if (dbResponse.IsSuccess)
            {
                Ok();
            }
            else
            {
                BadRequest(dbResponse.Message);
            }
        }


        public async Task<ActionResult> ReceiptChange(int? id)
        {
            if (id == null) return RedirectToAction("Record");
            var data = await _db.Selling.FindUpdateBillAsync(id.GetValueOrDefault(), _db).ConfigureAwait(false);
            if (data == null) return RedirectToAction("Record");
            return View(data);
        }

        //Post: Change Receipt
        [HttpPost]
        public async Task<int> ReceiptChange(SellingUpdatePostModel model)
        {
            var dbResponse = await _db.Selling.BillUpdated(model, _db);
            if (dbResponse.IsSuccess)
            {
                Ok();
                return model.SellingId;
            }
            else
            {
                BadRequest(dbResponse.Message);
                return 0;
            }


        }

        public async Task DeleteBill(int id)
        {
            var dbResponse = await _db.Selling.DeleteBillAsync(id, _db).ConfigureAwait(false);

            if (dbResponse.IsSuccess)
            {
                Ok();
            }
            else
            {
                BadRequest(dbResponse.Message);
            }
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
