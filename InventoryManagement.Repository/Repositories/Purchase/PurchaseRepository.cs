using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetNewSnAsync()
        {

            var sn = 0;
            if (await Context.Purchase.AnyAsync().ConfigureAwait(false))
            {
                sn = await Context.Purchase.MaxAsync(p => p == null ? 0 : p.PurchaseSn).ConfigureAwait(false);
            }

            return sn + 1;
        }

        public async Task<DbResponse<int>> AddCustomAsync(PurchaseViewModel model, IUnitOfWork db)
        {
            var response = new DbResponse<int>();
            var newStocks = model.Products.SelectMany(p => p.ProductStocks.Select(s => s)).ToList();

            var duplicateStocks = await db.ProductStocks.IsExistListAsync(newStocks).ConfigureAwait(false);

            if (duplicateStocks.Any(s => !s.IsSold))
            {
                response.Message = "Product code already exists";
                response.IsSuccess = false;
                return response;
            }

            var newPurchaseSn = await GetNewSnAsync().ConfigureAwait(false);
            var newPurchasePaymentSn = await db.PurchasePayments.GetNewSnAsync().ConfigureAwait(false);
            var purchase = new Purchase
            {
                RegistrationId = model.RegistrationId,
                VendorId = model.VendorId,
                PurchaseSn = newPurchaseSn,
                PurchaseTotalPrice = model.PurchaseTotalPrice,
                PurchaseDiscountAmount = model.PurchaseDiscountAmount,
                PurchasePaidAmount = model.PurchasePaidAmount,
                MemoNumber = model.MemoNumber,
                PurchaseDate = model.PurchaseDate.BdTime().Date,
                PurchaseList = model.Products.Select(p => new PurchaseList
                {
                    ProductId = p.ProductId,
                    Description = p.Description,
                    Warranty = p.Warranty,
                    Note = p.Note,
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    ProductStock = p.ProductStocks.Select(s => new ProductStock
                    {
                        ProductId = p.ProductId,
                        ProductCode = s.ProductCode
                    }).ToList()
                }).ToList(),
                PurchasePaymentList = model.PurchasePaidAmount > 0 ?
                    new List<PurchasePaymentList>
                    {
                        new PurchasePaymentList
                        {
                            PurchasePaidAmount = model.PurchasePaidAmount,
                            PurchasePayment = new PurchasePayment
                            {
                                PurchasePaymentId = 0,
                                RegistrationId = model.RegistrationId,
                                VendorId = model.VendorId,
                                ReceiptSn = newPurchasePaymentSn,
                                PaidAmount = model.PurchasePaidAmount,
                                PaymentMethod = model.PaymentMethod,
                                PaidDate = model.PurchaseDate.BdTime().Date,
                                AccountId = model.AccountId
                            }
                        }
                    } : null
            };

            await Context.Purchase.AddAsync(purchase).ConfigureAwait(false);


            //Update Product Info
            foreach (var item in model.Products)
            {
                var product = await Context.Product.FindAsync(item.ProductId);
                product.ProductId = item.ProductId;
                product.Description = item.Description;
                product.Warranty = item.Warranty;
                product.Note = item.Note;
                product.SellingPrice = item.SellingPrice;
                Context.Product.Update(product);
            }


            try
            {

                //Account subtract balance
                if (model.PurchasePaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceSubtract(model.AccountId.Value, model.PurchasePaidAmount);

                await Context.SaveChangesAsync().ConfigureAwait(false);

                db.Vendors.UpdatePaidDue(model.VendorId);

                //Product Logs 
                var logs = purchase.PurchaseList.SelectMany(p => p.ProductStock.Select(c => new ProductLogAddModel
                {
                    PurchaseId = p.PurchaseId,
                    ProductStockId = c.ProductStockId,
                    ActivityByRegistrationId = model.RegistrationId,
                    Details = $"Product Buy at Receipt No: {purchase.PurchaseSn}",
                    LogStatus = ProductLogStatus.Buy
                })).ToList();

                db.ProductLog.AddList(logs);

                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = purchase.PurchaseId;
            }
            catch (DbUpdateException e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public Task<PurchaseReceiptViewModel> PurchaseReceiptAsync(int id, IUnitOfWork db)
        {
            // var categoryList = db.ProductCatalogs.CatalogDll();

            var purchaseReceipt = Context.Purchase
                .Include(p => p.Vendor)
                .Include(p => p.Registration)
                .Include(p => p.PurchaseList)
                .ThenInclude(pl => pl.Product)
                .ThenInclude(pd => pd.ProductCatalog)
                .Include(p => p.PurchaseList)
                .ThenInclude(pl => pl.Product)
                .ThenInclude(pd => pd.ProductStock)
                .Include(p => p.PurchasePaymentList)
                .ThenInclude(p => p.PurchasePayment)
                .Select(p => new PurchaseReceiptViewModel
                {
                    PurchaseSn = p.PurchaseSn,
                    PurchaseId = p.PurchaseId,
                    PurchaseTotalPrice = p.PurchaseTotalPrice,
                    PurchaseDiscountAmount = p.PurchaseDiscountAmount,
                    PurchasePaidAmount = p.PurchasePaidAmount,
                    PurchaseDueAmount = p.PurchaseDueAmount,
                    PurchaseReturnAmount = p.PurchaseReturnAmount,
                    PurchaseDate = p.PurchaseDate,
                    MemoNumber = p.MemoNumber,
                    Products = p.PurchaseList.Select(pd => new ProductViewModel
                    {
                        PurchaseListId = pd.PurchaseListId,
                        ProductId = pd.ProductId,
                        ProductCatalogId = pd.Product.ProductCatalogId,
                        ProductCatalogName = pd.Product.ProductCatalog.CatalogName,
                        ProductName = pd.Product.ProductName,
                        Description = pd.Description,
                        Warranty = pd.Warranty,
                        PurchasePrice = pd.PurchasePrice,
                        SellingPrice = pd.SellingPrice,
                        ProductStocks = pd.ProductStock.Select(s => new ProductStockViewModel
                        {
                            ProductCode = s.ProductCode
                        }).ToList()
                    }).ToList(),
                    Payments = p.PurchasePaymentList.Select(pp => new PurchasePaymentListViewModel
                    {
                        PaymentMethod = pp.PurchasePayment.Account.AccountName,
                        PaidAmount = pp.PurchasePaidAmount,
                        PaidDate = pp.PurchasePayment.PaidDate
                    }).ToList(),
                    VendorInfo = new VendorViewModel
                    {
                        VendorId = p.Vendor.VendorId,
                        VendorCompanyName = p.Vendor.VendorCompanyName,
                        VendorName = p.Vendor.VendorName,
                        VendorAddress = p.Vendor.VendorAddress,
                        VendorPhone = p.Vendor.VendorPhone,
                        InsertDate = p.Vendor.InsertDate,
                        Due = p.Vendor.Due
                    },
                    InstitutionInfo = db.Institutions.FindCustom(),
                    SoildBy = p.Registration.Name
                }).FirstOrDefaultAsync(p => p.PurchaseId == id);

            return purchaseReceipt;
        }

        public DataResult<PurchaseRecordViewModel> Records(DataRequest request)
        {
            var r = Context.Purchase
                .Include(p => p.Vendor)
                .Select(p => new PurchaseRecordViewModel
                {
                    PurchaseId = p.PurchaseId,
                    VendorId = p.VendorId,
                    VendorCompanyName = p.Vendor.VendorCompanyName,
                    PurchaseSn = p.PurchaseSn,
                    PurchaseAmount = p.PurchaseTotalPrice,
                    PurchasePaidAmount = p.PurchasePaidAmount,
                    PurchaseDiscountAmount = p.PurchaseDiscountAmount,
                    PurchaseDueAmount = p.PurchaseDueAmount,
                    PurchaseDate = p.PurchaseDate,
                    MemoNumber = p.MemoNumber
                }).OrderBy(p => p.PurchaseDate);
            return r.ToDataResult(request);
        }

        public ICollection<int> Years()
        {
            var years = Context.Purchase
                .GroupBy(e => new
                {
                    e.PurchaseDate.Year
                })
                .Select(g => g.Key.Year)
                .OrderBy(o => o)
                .ToList();

            var currentYear = DateTime.Now.Year;

            if (!years.Contains(currentYear)) years.Add(currentYear);

            return years;
        }

        public decimal TotalDue()
        {
            return Context.Purchase?.Sum(p => p.PurchaseDueAmount) ?? 0;
        }

        public decimal DailyPurchaseAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now;
            return Context.Purchase.Where(s => s.PurchaseDate.Date == saleDate.Date)?
                       .Sum(s => s.PurchaseTotalPrice - s.PurchaseDiscountAmount) ?? 0;
        }

        public ICollection<MonthlyAmount> MonthlyAmounts(int year)
        {
            var months = Context.Purchase.Where(e => e.PurchaseDate.Year == year)
                .GroupBy(e => new
                {
                    number = e.PurchaseDate.Month

                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = g.Sum(e => e.PurchaseTotalPrice - e.PurchaseDiscountAmount)
                })
                .ToList();

            return months;
        }

        public async Task<DbResponse> UpdateMemoNumberAsync(int purchaseId, string newMemoNumber)
        {
            var response = new DbResponse();

            try
            {
                var purchase = await Context.Purchase.FindAsync(purchaseId).ConfigureAwait(false);
                purchase.MemoNumber = newMemoNumber;

                Context.Purchase.Update(purchase);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                response.IsSuccess = true;
                response.Message = "Success";
            }
            catch (DbUpdateException e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<DbResponse<PurchaseGetByReceiptModel>> GetDetailsByReceiptNo(int receipt)
        {
            try
            {
                var purchase = await Context.Purchase
                    .Include(p => p.PurchaseList)
                    .ThenInclude(l => l.Product)
                    .FirstOrDefaultAsync(p => p.PurchaseSn == receipt).ConfigureAwait(false);

                if (purchase == null) return new DbResponse<PurchaseGetByReceiptModel>(false, "Receipt Not Found");

                var data = new PurchaseGetByReceiptModel
                {
                    PurchaseAdjustedAmount = purchase.PurchasePaidAmount,
                    PurchaseDescription = string.Join(", ", purchase.PurchaseList.Select(p => $"{p.Product.ProductName}")),
                    PurchaseId = purchase.PurchaseId
                };

                return new DbResponse<PurchaseGetByReceiptModel>(true, "Success", data);
            }
            catch (DbUpdateException e)
            {
                return new DbResponse<PurchaseGetByReceiptModel>(false, e.Message);
            }

        }

        public Task<PurchaseUpdateGetModel> FindUpdateBillAsync(int id, IUnitOfWork db)
        {
            var purchase = Context.Purchase
              .Include(s => s.Vendor)
              .Include(s => s.PurchaseList)
              .ThenInclude(ps => ps.ProductStock)
              .ThenInclude(s => s.PurchaseList)
              .ThenInclude(p => p.Product)
              .ThenInclude(pd => pd.ProductCatalog)
              .Select(p => new PurchaseUpdateGetModel
              {

                  VendorPhone = p.Vendor.VendorPhone,
                  VendorId = p.VendorId,
                  VendorName = p.Vendor.VendorName,
                  VendorCompanyName = p.Vendor.VendorCompanyName,
                  PurchaseId = p.PurchaseId,
                  PurchaseSn = p.PurchaseSn,
                  MemoNumber = p.MemoNumber,
                  PurchaseTotalPrice = p.PurchaseTotalPrice,
                  PurchaseDiscountAmount = p.PurchaseDiscountAmount,
                  PurchaseDueAmount = p.PurchaseDueAmount,
                  PurchaseReturnAmount = p.PurchaseReturnAmount,
                  PurchasePaidAmount = p.PurchasePaidAmount,
                  PurchaseDate = p.PurchaseDate,
                  PurchaseList = p.PurchaseList.Select(l => new ProductViewModel
                  {
                      PurchaseListId = l.PurchaseListId,
                      ProductId = l.ProductId,
                      ProductName = l.Product.ProductName,
                      ProductCatalogId = l.Product.ProductCatalogId,
                      ProductCatalogName = l.Product.ProductCatalog.CatalogName,
                      Description = l.Description,
                      Warranty = l.Warranty,
                      Note = l.Note,
                      SellingPrice = l.SellingPrice,
                      PurchasePrice = l.PurchasePrice,
                      ProductStocks = l.ProductStock.Select(s => new ProductStockViewModel
                      {
                          ProductStockId = s.ProductStockId,
                          ProductCode = s.ProductCode,
                          IsSold = s.IsSold
                      }).ToList()
                  }).ToList()
              }).FirstOrDefaultAsync(s => s.PurchaseId == id);

            return purchase;
        }

        public async Task<DbResponse<int>> BillUpdated(PurchaseUpdatePostModel model, IUnitOfWork db, string userName)
        {
            var response = new DbResponse<int>();
            try
            {
                var purchase = Context.Purchase
                    .Include(s => s.PurchaseList)
                    .FirstOrDefault(s => s.PurchaseId == model.PurchaseId);

                if (purchase == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Not found";
                    return response;
                }
                var registrationId = db.Registrations.GetRegID_ByUserName(userName);
                var returnRecord = new PurchasePaymentReturnRecordModel
                {
                    PrevReturnAmount = decimal.Round(purchase.PurchaseReturnAmount,2),
                    CurrentReturnAmount = decimal.Round(model.PurchaseReturnAmount, 2),
                    AccountId = model.AccountId,
                    PurchaseId = purchase.PurchaseId,
                    RegistrationId = registrationId
                };

                purchase.PurchaseTotalPrice = decimal.Round(model.PurchaseTotalPrice, 2);
                purchase.PurchaseDiscountAmount = decimal.Round(model.PurchaseDiscountAmount, 2);
                purchase.PurchaseReturnAmount = decimal.Round(model.PurchaseReturnAmount, 2);
                purchase.PurchasePaidAmount += decimal.Round(model.PaidAmount, 2);


                var due = (purchase.PurchaseTotalPrice + purchase.PurchaseReturnAmount) - (purchase.PurchaseDiscountAmount + purchase.PurchasePaidAmount);
                if (due < 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Due cannot be less than zero";
                    return response;
                }

                if (model.RemovedProductStockIds != null)
                {
                    if (model.RemovedProductStockIds.Any())
                    {
                        var removedStocks = Context.ProductStock
                            .Include(s => s.ProductLog)
                            .Include(s => s.Warranty)
                            .Where(s => model.RemovedProductStockIds.Contains(s.ProductStockId)).ToList();

                        if (removedStocks.Any(s => s.IsSold))
                        {
                            response.IsSuccess = false;
                            response.Message = "Sold product can not be return";
                            return response;
                        }

                        if (removedStocks.Any()) Context.ProductStock.RemoveRange(removedStocks);
                    }
                }


                if (model.PurchaseList == null)
                {
                    purchase.PurchaseList = null;
                }

                Context.Purchase.Update(purchase);

                var existProduct = model.PurchaseList.Where(p => p.PurchaseListId != 0 && p.AddedProductCodes != null).ToList();


                foreach (var pItem in existProduct)
                {
                    var purchaseList = Context.PurchaseList.Find(pItem.PurchaseListId);

                    purchaseList.SellingPrice = pItem.SellingPrice;
                    purchaseList.PurchasePrice = pItem.PurchasePrice;
                    purchaseList.Description = pItem.Description;
                    purchaseList.Note = pItem.Note;
                    purchaseList.Warranty = pItem.Warranty;
                    purchaseList.ProductStock = pItem.AddedProductCodes.Select(s => new ProductStock
                    {
                        ProductId = pItem.ProductId,
                        ProductCode = s
                    }).ToList();

                    Context.PurchaseList.Update(purchaseList);
                }

                var newProduct = model.PurchaseList.Where(p => p.PurchaseListId == 0 && p.AddedProductCodes != null).ToList();

                var newPurchaseList = newProduct.Select(p => new PurchaseList
                {
                    PurchaseListId = p.PurchaseListId,
                    PurchaseId = model.PurchaseId,
                    ProductId = p.ProductId,
                    SellingPrice = p.SellingPrice,
                    PurchasePrice = p.PurchasePrice,
                    Description = p.Description,
                    Note = p.Note,
                    Warranty = p.Warranty,
                    ProductStock = p.AddedProductCodes.Select(s => new ProductStock
                    {
                        ProductId = p.ProductId,
                        ProductCode = s
                    }).ToList()
                }).ToList();

                Context.PurchaseList.AddRange(newPurchaseList);



                if (model.PaidAmount > 0)
                {
                    var newSellingPaymentSn = await db.PurchasePayments.GetNewSnAsync().ConfigureAwait(false);
                    var payment = new PurchasePayment
                    {
                        RegistrationId = registrationId,
                        VendorId = purchase.VendorId,
                        ReceiptSn = newSellingPaymentSn,
                        PaidAmount = model.PaidAmount,
                        AccountId = model.AccountId,
                        PaidDate = DateTime.Now.BdTime().Date,
                        PurchasePaymentList = new List<PurchasePaymentList>
                        {
                            new PurchasePaymentList
                            {
                                PurchasePaidAmount = model.PaidAmount,
                                PurchaseId =  model.PurchaseId
                            }
                        }
                    };

                    await Context.PurchasePayment.AddAsync(payment);
                }

                //Account add balance
                if (model.PaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceSubtract(model.AccountId.Value, model.PaidAmount);


                await Context.SaveChangesAsync();

                db.Vendors.UpdatePaidDue(purchase.VendorId);

                //remove purchase list
                var removedPurchaseList = Context.PurchaseList.Where(l => !l.ProductStock.Any()).ToList();

                Context.RemoveRange(removedPurchaseList);
                await Context.SaveChangesAsync();

                //Product Logs 
                var codes = model.PurchaseList.SelectMany(l => l.AddedProductCodes).ToArray();
                var stockList = Context
                    .ProductStock
                    .Include(p => p.PurchaseList)
                    .Where(s => s.PurchaseList.PurchaseId == model.PurchaseId && codes.Contains(s.ProductCode))
                    .ToList();

                if (stockList.Any())
                {
                    var productLogs = stockList.Select(c => new ProductLogAddModel
                    {
                        PurchaseId = model.PurchaseId,
                        ProductStockId = c.ProductStockId,
                        ActivityByRegistrationId = registrationId,
                        Details = $"Bill Updated: Product Buy at Receipt No: {purchase.PurchaseSn}",
                        LogStatus = ProductLogStatus.PurchaseUpdate
                    }).ToList();
                    //Product log
                    db.ProductLog.AddList(productLogs);
                }

                //Return amount account update 
                db.Account.PurchaseReturnRecordAdd(returnRecord);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Success";
            response.Data = model.PurchaseId;

            return response;
        }
    }
}