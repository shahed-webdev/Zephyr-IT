using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class SellingRepository : Repository<Selling>, ISellingRepository
    {
        public SellingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetNewSnAsync()
        {
            var sn = 0;
            if (await Context.Selling.AnyAsync().ConfigureAwait(false))
            {
                sn = await Context.Selling.MaxAsync(s => s == null ? 0 : s.SellingSn).ConfigureAwait(false);
            }

            return sn + 1;
        }

        public async Task<DbResponse<int>> AddCustomAsync(SellingViewModel model, IUnitOfWork db)
        {
            var response = new DbResponse<int>();
            var codes = model.ProductList.SelectMany(p => p.ProductCodes).ToArray();
            var StockOuts = await db.ProductStocks.IsStockOutAsync(codes).ConfigureAwait(false);
            if (StockOuts.Length > 0)
            {
                response.Message = "Product Stock Out";
                response.IsSuccess = false;
                return response;
            }

            var newSellingSn = await GetNewSnAsync().ConfigureAwait(false);
            var newSellingPaymentSn = await db.SellingPayments.GetNewSnAsync().ConfigureAwait(false);
            var sellingStock = await db.ProductStocks.SellingStockFromCodesAsync(codes);

            var selling = new Selling
            {
                RegistrationId = model.RegistrationId,
                CustomerId = model.CustomerId,
                SellingSn = newSellingSn,
                SellingTotalPrice = model.SellingTotalPrice,
                SellingDiscountAmount = model.SellingDiscountAmount,
                SellingDiscountPercentage = model.SellingDiscountAmount,
                SellingPaidAmount = model.SellingPaidAmount,
                SellingDate = model.SellingDate.ToLocalTime(),
                SellingList = model.ProductList.Select(l => new SellingList
                {
                    ProductId = l.ProductId,
                    SellingPrice = l.SellingPrice,
                    Description = l.Description,
                    Warranty = l.Warranty,
                    ProductStock = sellingStock.Where(s => l.ProductCodes.Contains(s.ProductCode)).Select(s =>
                    {
                        s.IsSold = true;
                        return s;
                    }).ToList()
                }).ToList(),
                SellingPaymentList = model.SellingPaidAmount > 0 ?
                    new List<SellingPaymentList>
                    {
                        new SellingPaymentList
                        {
                            SellingPaidAmount = model.SellingPaidAmount,
                            SellingPayment = new SellingPayment
                            {
                                SellingPaymentId = 0,
                                RegistrationId = model.RegistrationId,
                                CustomerId  = model.CustomerId,
                                ReceiptSn = newSellingPaymentSn,
                                PaidAmount = model.SellingPaidAmount,
                                PaymentMethod = model.PaymentMethod,
                                PaidDate = model.SellingDate.ToLocalTime()
                            }
                        }
                    } : null
            };


            await Context.Selling.AddAsync(selling).ConfigureAwait(false);
            try
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);

                db.Customers.UpdatePaidDue(model.CustomerId);

                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = selling.SellingId;
            }
            catch (DbUpdateException e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public Task<SellingReceiptViewModel> SellingReceiptAsync(int id, IUnitOfWork db)
        {
            var sellingReceipt = Context.Selling
              .Include(s => s.Customer)
              .ThenInclude(c => c.CustomerPhone)
              .Include(s => s.Registration)
              .Include(s => s.SellingList)
              .ThenInclude(ps => ps.ProductStock)
              .ThenInclude(p => p.Product)
              .ThenInclude(pd => pd.ProductCatalog)
              .Include(s => s.SellingPaymentList)
              .ThenInclude(sp => sp.SellingPayment)
              .Select(s => new SellingReceiptViewModel
              {
                  SellingSn = s.SellingSn,
                  SellingId = s.SellingId,
                  SellingTotalPrice = s.SellingTotalPrice,
                  SellingDiscountAmount = s.SellingDiscountAmount,
                  SellingPaidAmount = s.SellingPaidAmount,
                  SellingDueAmount = s.SellingDueAmount,
                  SellingReturnAmount = s.SellingReturnAmount,
                  SellingDate = s.SellingDate,
                  Products = s.SellingList.Select(pd => new SellingReceiptProductViewModel
                  {
                      ProductId = pd.Product.ProductId,
                      ProductCatalogId = pd.Product.ProductCatalogId,
                      ProductCatalogName = pd.Product.ProductCatalog.CatalogName,
                      ProductName = pd.Product.ProductName,
                      Description = pd.Description,
                      Warranty = pd.Warranty,
                      SellingPrice = pd.SellingPrice,
                      ProductCodes = pd.ProductStock.Select(ss => ss.ProductCode).ToArray()
                  }).ToList(),
                  Payments = s.SellingPaymentList.Select(pp => new SellingPaymentViewModel
                  {
                      PaymentMethod = pp.SellingPayment.PaymentMethod,
                      PaidAmount = pp.SellingPaidAmount,
                      PaidDate = pp.SellingPayment.PaidDate
                  }).ToList(),
                  CustomerInfo = new CustomerReceiptViewModel
                  {
                      CustomerId = s.Customer.CustomerId,
                      Name = s.Customer.IsIndividual ? s.Customer.CustomerName : s.Customer.OrganizationName,
                      CustomerAddress = s.Customer.CustomerAddress,
                      CustomerPhone = s.Customer.CustomerPhone.FirstOrDefault().Phone
                  },
                  InstitutionInfo = db.Institutions.FindCustom(),
                  SoldBy = s.Registration.Name
              }).FirstOrDefaultAsync(s => s.SellingId == id);

            return sellingReceipt;
        }

        public DataResult<SellingRecordViewModel> Records(DataRequest request)
        {
            var r = Context.Selling.Include(s => s.Customer).Select(s => new SellingRecordViewModel
            {
                SellingId = s.SellingId,
                CustomerId = s.CustomerId,
                CustomerName = s.Customer.CustomerName,
                SellingSn = s.SellingSn,
                SellingAmount = s.SellingTotalPrice,
                SellingPaidAmount = s.SellingPaidAmount,
                SellingDiscountAmount = s.SellingDiscountAmount,
                SellingDueAmount = s.SellingDueAmount,
                SellingDate = s.SellingDate
            });
            return r.ToDataResult(request);
        }

        public ICollection<int> Years()
        {
            var years = Context.Selling
                .GroupBy(e => new
                {
                    e.SellingDate.Year
                })
                .Select(g => g.Key.Year)
                .OrderBy(o => o)
                .ToList();

            var currentYear = DateTime.Now.Year;

            if (!years.Contains(currentYear)) years.Add(currentYear);

            return years;
        }

        public double TotalDue()
        {
            return Context.Selling?.Sum(s => s.SellingDueAmount) ?? 0;
        }

        public double DailySaleAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now;
            return Context.Selling.Where(s => s.SellingDate.Date == saleDate.Date)?
                  .Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount) ?? 0;
        }

        public double DailyProfit(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now;
            return (from selling in Context.Selling
                    join sellingList in Context.SellingList on selling.SellingId equals sellingList.SellingId
                    join productStock in Context.ProductStock on sellingList.SellingListId equals productStock.SellingListId
                    join purchaseList in Context.PurchaseList on productStock.PurchaseListId equals purchaseList
                        .PurchaseListId
                    where selling.SellingDate.Date == saleDate.Date
                    select sellingList.SellingPrice - purchaseList.PurchasePrice - ((purchaseList.PurchasePrice * purchaseList.Purchase.PurchaseDiscountPercentage) / 100))?.Sum() ?? 0;

        }

        public double DailySoldPurchaseAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now;
            //return Context.Selling
            //    .Include(s => s.SellingList)
            //    .ThenInclude(l => l.ProductStock)
            //    .ThenInclude(p => p.PurchaseList)
            //    .Where(s => s.SellingDate.Date == saleDate.Date)?.Sum(s =>
            //        s.SellingList.Sum(l => l.ProductStock.Sum(p => p.PurchaseList.PurchasePrice))) ?? 0;
            return 0;
        }

        public ICollection<MonthlyAmount> MonthlyAmounts(int year)
        {
            var months = Context.Selling.Where(e => e.SellingDate.Year == year)
                .GroupBy(e => new
                {
                    number = e.SellingDate.Month

                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = g.Sum(e => e.SellingTotalPrice - e.SellingDiscountAmount)
                })
                .ToList();

            return months;
        }

        public ICollection<MonthlyAmount> MonthlyProfit(int year)
        {
            var months = (from selling in Context.Selling
                          join sellingList in Context.SellingList on selling.SellingId equals sellingList.SellingId
                          join productStock in Context.ProductStock on sellingList.SellingListId equals productStock.SellingListId
                          join purchaseList in Context.PurchaseList on productStock.PurchaseListId equals purchaseList
                              .PurchaseListId
                          where selling.SellingDate.Year == year
                          select new
                          {
                              MonthNumer = selling.SellingDate.Month,
                              profit = sellingList.SellingPrice - purchaseList.PurchasePrice - ((purchaseList.PurchasePrice * purchaseList.Purchase.PurchaseDiscountPercentage) / 100)
                          }
                     ).GroupBy(e => new
                     {
                         number = e.MonthNumer

                     })
                     .Select(g => new MonthlyAmount
                     {
                         MonthNumber = g.Key.number,
                         Amount = g.Sum(s => s.profit)
                     })
                     .ToList();



            return months;
        }

        public async Task<DbResponse> DeleteBillAsync(int id, IUnitOfWork db)
        {
            var response = new DbResponse();
            try
            {
                var selling = Context.Selling
                    .Include(s => s.SellingList)
                    .Include(s => s.SellingPaymentList)
                    .FirstOrDefault(s => s.SellingId == id);

                if (selling == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Not found";
                    return response;
                }

                if (selling.SellingPaymentList.Count > 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Payment Exist";
                    return response;
                }


                var stocks = selling.SellingList.SelectMany(s => s.ProductStock).Select(sp =>
                 {
                     sp.IsSold = false;
                     return sp;
                 }).ToList();

                Context.ProductStock.UpdateRange(stocks);
                Context.Selling.Remove(selling);
                Context.SaveChanges();
                db.Customers.UpdatePaidDue(selling.CustomerId);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Success";
            return response;
        }

        public Task<SellingUpdateGetModel> FindUpdateBillAsync(int id, IUnitOfWork db)
        {
            var sellingReceipt = Context.Selling
              .Include(s => s.Customer)
              .ThenInclude(c => c.CustomerPhone)
              .Include(s => s.Registration)
              .Include(s => s.SellingList)
              .ThenInclude(ps => ps.ProductStock)
              .ThenInclude(s => s.PurchaseList)
              .ThenInclude(p => p.Product)
              .ThenInclude(pd => pd.ProductCatalog)
              .Include(s => s.SellingPaymentList)
              .ThenInclude(sp => sp.SellingPayment)
              .Select(s => new SellingUpdateGetModel
              {
                  SellingSn = s.SellingSn,
                  SellingId = s.SellingId,
                  SellingTotalPrice = s.SellingTotalPrice,
                  SellingDiscountAmount = s.SellingDiscountAmount,
                  SellingPaidAmount = s.SellingPaidAmount,
                  SellingDueAmount = s.SellingDueAmount,
                  SellingReturnAmount = s.SellingReturnAmount,
                  SellingDate = s.SellingDate,
                  Products = s.SellingList.Select(pd => new SellingReceiptProductViewModel
                  {
                      SellingListId = pd.SellingListId,
                      SellingId = pd.SellingId,
                      ProductId = pd.Product.ProductId,
                      ProductCatalogId = pd.Product.ProductCatalogId,
                      ProductCatalogName = pd.Product.ProductCatalog.CatalogName,
                      ProductName = pd.Product.ProductName,
                      Description = pd.Description,
                      Note = pd.Product.Note,
                      Warranty = pd.Warranty,
                      SellingPrice = pd.SellingPrice,
                      PurchasePrice = pd.ProductStock.FirstOrDefault().PurchaseList.PurchasePrice,
                      ProductCodes = pd.ProductStock.Select(ss => ss.ProductCode).ToArray()
                  }).ToList(),
                  Payments = s.SellingPaymentList.Select(pp => new SellingPaymentViewModel
                  {
                      PaymentMethod = pp.SellingPayment.PaymentMethod,
                      PaidAmount = pp.SellingPaidAmount,
                      PaidDate = pp.SellingPayment.PaidDate
                  }).ToList(),
                  CustomerInfo = new CustomerReceiptViewModel
                  {
                      CustomerId = s.Customer.CustomerId,
                      Name = s.Customer.IsIndividual ? s.Customer.CustomerName : s.Customer.OrganizationName,
                      CustomerAddress = s.Customer.CustomerAddress,
                      CustomerPhone = s.Customer.CustomerPhone.FirstOrDefault().Phone
                  },
                  SoildBy = s.Registration.Name
              }).FirstOrDefaultAsync(s => s.SellingId == id);

            return sellingReceipt;
        }

        public async Task<DbResponse> BillUpdated(SellingUpdatePostModel model, IUnitOfWork db)
        {
            var response = new DbResponse();
            try
            {
                var stocks = new List<ProductStock>();
                if (model.AddedProductCodes.Any())
                {
                    var addedStocks = await db.ProductStocks.SellingStockFromCodesAsync(model.AddedProductCodes);
                    addedStocks = addedStocks.Select(s =>
                    {
                        s.IsSold = true;
                        return s;
                    }).ToList();
                    stocks.AddRange(addedStocks);
                }

                var selling = Context.Selling.Include(s => s.SellingList).FirstOrDefault(s => s.SellingId == model.SellingId);
                if (selling == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Not found";
                    return response;
                }

                selling.SellingTotalPrice = model.SellingTotalPrice;
                selling.SellingDiscountAmount = model.SellingDiscountAmount;
                selling.SellingReturnAmount = model.SellingReturnAmount;
                selling.SellingPaidAmount += model.PaidAmount;

                var due = (selling.SellingTotalPrice + selling.SellingReturnAmount) - (selling.SellingDiscountAmount + selling.SellingPaidAmount);
                if (due < 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Due cannot be less than zero";
                    return response;
                }

                selling.SellingList = model.Products.Select(p => new SellingList
                {
                    SellingListId = p.SellingListId,
                    SellingId = model.SellingId,
                    ProductId = p.ProductId,
                    SellingPrice = p.SellingPrice,
                    Description = p.Description,
                    Warranty = p.Warranty,
                    ProductStock = stocks.Where(s => p.RemainCodes.Contains(s.ProductCode)).ToList()
                }).ToList();


                if (model.RemovedProductCodes.Any())
                {
                    var removedStocks = await db.ProductStocks.SellingStockFromCodesAsync(model.RemovedProductCodes);

                    removedStocks = removedStocks.Select(s =>
                    {
                        s.IsSold = false;
                        s.SellingListId = null;
                        return s;
                    }).ToList();

                    if (removedStocks.Any())
                    {
                        Context.ProductStock.UpdateRange(removedStocks);
                    }
                }


                Context.Selling.Update(selling);

                if (model.PaidAmount > 0)
                {
                    var newSellingPaymentSn = await db.SellingPayments.GetNewSnAsync().ConfigureAwait(false);
                    var payment = new SellingPayment
                    {
                        RegistrationId = model.UpdateRegistrationId,
                        CustomerId = selling.CustomerId,
                        ReceiptSn = newSellingPaymentSn,
                        PaidAmount = model.PaidAmount,
                        PaymentMethod = model.PaymentMethod,
                        PaidDate = model.PaidDate,
                        SellingPaymentList = new List<SellingPaymentList>
                        {
                            new SellingPaymentList
                            {
                                SellingPaidAmount = model.PaidAmount,
                                SellingId =  model.SellingId
                            }
                        }
                    };

                    Context.SellingPayment.Add(payment);
                }

                await Context.SaveChangesAsync();

                db.Customers.UpdatePaidDue(selling.CustomerId);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Success";
            return response;
        }
    }
}