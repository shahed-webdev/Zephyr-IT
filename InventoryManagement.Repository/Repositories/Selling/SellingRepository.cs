using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        protected readonly IMapper _mapper;
        public SellingRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
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
            var accountCostPercentage = db.Account.GetCostPercentage(model.AccountId.GetValueOrDefault());
            var selling = new Selling
            {
                RegistrationId = model.RegistrationId,
                CustomerId = model.CustomerId,
                SellingSn = newSellingSn,
                SellingTotalPrice = model.SellingTotalPrice,
                SellingDiscountAmount = model.SellingDiscountAmount,
                SellingPaidAmount = model.SellingPaidAmount,
                SellingDate = DateTime.Now.BdTime().Date,
                LastUpdateDate = DateTime.Now.BdTime().Date,
                SellingList = model.ProductList.Select(l => new SellingList
                {
                    ProductId = l.ProductId,
                    SellingPrice = l.SellingPrice,
                    PurchasePrice = l.PurchasePrice,
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
                                PaidDate = DateTime.Now.BdTime().Date,
                                AccountId = model.AccountId,
                                AccountCost = accountCostPercentage
                            }
                        }
                    } : null,
                BuyingTotalPrice = model.ProductList.Sum(p => p.PurchasePrice * p.ProductCodes.Length),
                PromisedPaymentDate = model.PromisedPaymentDate,
                ExpenseTotal = model.Expense,
                SellingExpense = model.Expense > 0 ? new List<SellingExpense>
                {
                    new SellingExpense
                    {
                        Expense = model.Expense,
                        ExpenseDescription = model.ExpenseDescription,
                        InsertDateUtc = DateTime.Now.BdTime()
                    }
                } : null,
                ServiceCost = model.ServiceCost,
                ServiceCharge = model.ServiceCharge,
                ServiceChargeDescription = model.ServiceChargeDescription,
                SellingAccountCost = model.SellingPaidAmount * accountCostPercentage / 100
            };


            await Context.Selling.AddAsync(selling).ConfigureAwait(false);
            try
            {
                //Account add balance
                if (model.SellingPaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceAdd(model.AccountId.Value, model.SellingPaidAmount);

                //Sales parson add balance
                if (model.SellingPaidAmount > 0)
                    db.Registrations.BalanceAdd(model.RegistrationId, model.SellingPaidAmount);

                await Context.SaveChangesAsync().ConfigureAwait(false);

                db.Customers.UpdatePaidDue(model.CustomerId);

                //Product Logs 
                var logs = selling.SellingList.SelectMany(p => p.ProductStock.Select(c => new ProductLogAddModel
                {
                    ProductStockId = c.ProductStockId,
                    ActivityByRegistrationId = model.RegistrationId,
                    Details = $"Product Selling at Receipt No: {selling.SellingSn}",
                    LogStatus = ProductLogStatus.Sale
                })).ToList();

                db.ProductLog.AddList(logs);

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
              .ThenInclude(a => a.Account)
              .Include(s => s.SellingExpense)
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
                      PaymentMethod = pp.SellingPayment.Account.AccountName,
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
                  SoldBy = s.Registration.Name,
                  PromisedPaymentDate = s.PromisedPaymentDate,
                  ServiceCharge = s.ServiceCharge,
                  ServiceChargeDescription = s.ServiceChargeDescription,
                  SellingExpenses = s.SellingExpense.Select(e => new SellingExpenseListModel
                  {
                      SellingExpenseId = e.SellingExpenseId,
                      Expense = e.Expense,
                      ExpenseDescription = e.ExpenseDescription,
                      InsertDateUtc = e.InsertDateUtc
                  }).ToList()

              }).FirstOrDefaultAsync(s => s.SellingId == id);

            return sellingReceipt;
        }

        public DataResult<SellingRecordViewModel> Records(DataRequest request)
        {
            var r = Context.Selling
                .Include(s => s.Customer)
                .Include(s => s.Registration)
                .Select(s => new SellingRecordViewModel
                {
                    SellingId = s.SellingId,
                    RegistrationId = s.RegistrationId,
                    BillCreateBy = $" {s.Registration.Name} ({s.Registration.UserName})",
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.CustomerName,
                    SellingSn = s.SellingSn,
                    SellingAmount = s.SellingTotalPrice,
                    ServiceCharge = s.ServiceCharge,
                    SellingPaidAmount = s.SellingPaidAmount,
                    SellingDiscountAmount = s.SellingDiscountAmount,
                    SellingDueAmount = s.SellingDueAmount,
                    SellingDate = s.SellingDate,
                    LastUpdateDate = s.LastUpdateDate.Value,
                    PromisedPaymentDate = s.PromisedPaymentDate
                });
            return r.ToDataResult(request);
        }

        public DataResult<SellingRecordViewModel> DueRecords(DataRequest request)
        {
            var r = Context.Selling
                .Include(s => s.Customer)
                .Include(s => s.Registration)
                .Where(s => s.SellingDueAmount > 0)
                .OrderBy(s => s.PromisedPaymentDate)
                .Select(s => new SellingRecordViewModel
                {
                    SellingId = s.SellingId,
                    CustomerId = s.CustomerId,
                    RegistrationId = s.RegistrationId,
                    BillCreateBy = $" {s.Registration.Name} ({s.Registration.UserName})",
                    CustomerName = s.Customer.CustomerName,
                    SellingSn = s.SellingSn,
                    SellingAmount = s.SellingTotalPrice,
                    ServiceCharge = s.ServiceCharge,
                    SellingPaidAmount = s.SellingPaidAmount,
                    SellingDiscountAmount = s.SellingDiscountAmount,
                    SellingDueAmount = s.SellingDueAmount,
                    SellingDate = s.SellingDate,
                    LastUpdateDate = s.LastUpdateDate.Value,
                    PromisedPaymentDate = s.PromisedPaymentDate
                });
            return r.ToDataResult(request);
        }

        public DataResult<SellingBillProfitModel> BillWiseProfits(DataRequest request)
        {
            return Context.Selling
                .Include(s => s.Customer)
                .Where(s => s.SellingPaymentStatus == "Paid")
                .OrderBy(s => s.SellingSn)
                .ProjectTo<SellingBillProfitModel>(_mapper.ConfigurationProvider)
                .ToDataResult(request);
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

            var currentYear = DateTime.Now.BdTime().Year;

            if (!years.Contains(currentYear)) years.Add(currentYear);

            return years;
        }

        public decimal TotalDue()
        {
            return Context.Selling?.Sum(s => s.SellingDueAmount) ?? 0;
        }

        /// <summary>Calculate Report By (Total Amount – discount) Totally Payment competed date
        /// </summary>
        public decimal DailySaleAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now.BdTime();
            return Context.Selling.Where(s => s.LastUpdateDate == saleDate.Date)?
                  .Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount) ?? 0;
        }

        public decimal SaleAmountDateWise(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 12, 31);
            return Context
                .Selling
                .Where(r => r.LastUpdateDate <= eD && r.LastUpdateDate >= sD)
                .Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount);
        }

        public DbResponse<DateWiseSaleSummary> ProductSoldAmountDateWise(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var sD = fromDate ?? new DateTime(1000, 1, 1);
                var eD = toDate ?? new DateTime(3000, 12, 31);

                var summary = Context.Selling
                                  .Where(s => s.SellingDate <= eD && s.SellingDate >= sD)
                                  .GroupBy(s => true)
                                  .Select(g => new DateWiseSaleSummary
                                  {
                                      SoldAmount = g.Sum(e => e.SellingTotalPrice),
                                      DiscountAmount = g.Sum(e => e.SellingDiscountAmount),
                                      DueAmount = g.Sum(e => e.SellingDueAmount),
                                      ReceivedAmount = g.Sum(e => e.SellingPaidAmount)
                                  }).FirstOrDefault() ?? new DateWiseSaleSummary();

                return new DbResponse<DateWiseSaleSummary>(true, "Success", summary);
            }
            catch (Exception e)
            {
                return new DbResponse<DateWiseSaleSummary>(false, e.Message);
            }
        }

        public DbResponse<SellingBillProfitSummary> SellingBillProfitSummaryDateWise(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var sD = fromDate ?? new DateTime(1000, 1, 1);
                var eD = toDate ?? new DateTime(3000, 12, 31);

                var summary = Context.Selling
                    .Where(s => s.SellingPaymentStatus == "Paid" && s.LastUpdateDate <= eD && s.LastUpdateDate >= sD)
                    .GroupBy(s => true)
                    .Select(g => new SellingBillProfitSummary
                    {
                        ServiceProfit = g.Sum(e => e.ServiceProfit),
                        GrandProfit = g.Sum(e => e.GrandProfit)
                    }).FirstOrDefault() ?? new SellingBillProfitSummary();


                var exTransportation = Context.ExpenseTransportation
                    .Where(e => e.IsApproved && e.ExpenseDate <= eD && e.ExpenseDate >= sD).Sum(e => e.TotalExpense);

                var exGeneral = Context.Expense.Include(e => e.ExpenseCategory)
                    .Where(e => e.IsApproved && e.ExpenseDate <= eD && e.ExpenseDate >= sD).Sum(e => e.ExpenseAmount);

                summary.GenuineExpense = exTransportation + exGeneral;
                summary.NetProfit = summary.GrandProfit - summary.GenuineExpense;

                return new DbResponse<SellingBillProfitSummary>(true, "Success", summary);
            }
            catch (Exception e)
            {
                return new DbResponse<SellingBillProfitSummary>(false, e.Message);
            }
        }

        /// <summary>Calculate Report By (Total Amount – discount) Selling date
        /// </summary>
        public decimal DailyProductSoldAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now.BdTime();
            return Context.Selling.Where(s => s.SellingDate == saleDate.Date)?
                       .Sum(s => s.SellingTotalPrice - s.SellingDiscountAmount) ?? 0;
        }

        public decimal DailyProfit(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now.BdTime();
            return (from selling in Context.Selling
                    where selling.LastUpdateDate == saleDate.Date && selling.SellingPaymentStatus == "Paid"
                    select selling.GrandProfit)?.Sum() ?? 0;

            //join sellingList in Context.SellingList on selling.SellingId equals sellingList.SellingId
            //join productStock in Context.ProductStock on sellingList.SellingListId equals productStock.SellingListId
            //join purchaseList in Context.PurchaseList on productStock.PurchaseListId equals purchaseList
            //    .PurchaseListId
            // where selling.LastUpdateDate == saleDate.Date && selling.SellingPaymentStatus == "Paid"
            // select (sellingList.SellingPrice - (sellingList.SellingPrice * sellingList.Selling.SellingDiscountPercentage) / 100) -
            //   (purchaseList.PurchasePrice - ((purchaseList.PurchasePrice * purchaseList.Purchase.PurchaseDiscountPercentage) / 100)))?.Sum() ?? 0;

        }

        public decimal DailySoldPurchaseAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now.BdTime();
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
            var months = Context.Selling.Where(e => e.LastUpdateDate.Value.Year == year)
                .GroupBy(e => new
                {
                    number = e.LastUpdateDate.Value.Month

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
                          where selling.LastUpdateDate.Value.Year == year && selling.SellingPaymentStatus == "Paid"
                          select new
                          {
                              MonthNumer = selling.LastUpdateDate.Value.Month,
                              profit = selling.GrandProfit
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
              .ThenInclude(a => a.Account)
              .Include(s => s.SellingExpense)
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
                      PaymentMethod = pp.SellingPayment.Account.AccountName,
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
                  SoildBy = s.Registration.Name,
                  PromisedPaymentDate = s.PromisedPaymentDate,
                  ServiceCharge = s.ServiceCharge,
                  ServiceChargeDescription = s.ServiceChargeDescription,
                  SellingExpenses = s.SellingExpense.Select(e => new SellingExpenseListModel
                  {
                      SellingExpenseId = e.SellingExpenseId,
                      Expense = e.Expense,
                      ExpenseDescription = e.ExpenseDescription,
                      InsertDateUtc = e.InsertDateUtc
                  }).ToList(),
                  ExpenseTotal = s.ExpenseTotal,
                  ServiceCost = s.ServiceCost
              }).FirstOrDefaultAsync(s => s.SellingId == id);

            return sellingReceipt;
        }

        public async Task<DbResponse<int>> BillUpdated(SellingUpdatePostModel model, IUnitOfWork db)
        {
            var response = new DbResponse<int>();
            try
            {
                var stocks = new List<ProductStock>();

                var accountCostPercentage = db.Account.GetCostPercentage(model.AccountId.GetValueOrDefault());
                if (model.AddedProductCodes != null)
                {
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
                selling.SellingAccountCost += model.PaidAmount * accountCostPercentage / 100;
                selling.LastUpdateDate = DateTime.Now.BdTime().Date;
                selling.ServiceCharge = model.ServiceCharge;
                selling.ServiceCost = model.ServiceCost;
                selling.ServiceChargeDescription = model.ServiceChargeDescription;

                if (model.PromisedPaymentDate != null)
                    selling.PromisedPaymentDate = model.PromisedPaymentDate.Value.BdTime().Date;

                var due = (selling.SellingTotalPrice + selling.SellingReturnAmount + selling.ServiceCharge) - (selling.SellingDiscountAmount + selling.SellingPaidAmount);
                if (due < 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Due cannot be less than zero";
                    return response;
                }

                if (model.Products != null)
                {
                    selling.SellingList = model.Products.Select(p => new SellingList
                    {
                        SellingListId = p.SellingListId,
                        SellingId = model.SellingId,
                        ProductId = p.ProductId,
                        SellingPrice = p.SellingPrice,
                        PurchasePrice = p.PurchasePrice,
                        Description = p.Description,
                        Warranty = p.Warranty,
                        ProductStock = stocks.Where(s => p.RemainCodes.Contains(s.ProductCode)).ToList()
                    }).ToList();

                    selling.BuyingTotalPrice = model.Products.Sum(p => p.PurchasePrice * p.RemainCodes.Length);
                }



                if (model.RemovedProductCodes != null)
                {
                    if (model.RemovedProductCodes.Any())
                    {
                        var removedStocks =
                            await db.ProductStocks.SellingStockFromCodesAsync(model.RemovedProductCodes);

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
                        AccountId = model.AccountId,
                        PaidDate = DateTime.Now.BdTime().Date,
                        AccountCostPercentage = accountCostPercentage,
                        SellingPaymentList = new List<SellingPaymentList>
                        {
                            new SellingPaymentList
                            {
                                SellingPaidAmount = model.PaidAmount,
                                SellingId =  model.SellingId
                            }
                        }
                    };

                    await Context.SellingPayment.AddAsync(payment);
                }

                //Account add balance
                if (model.PaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceAdd(model.AccountId.Value, model.PaidAmount);

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
            response.Data = model.SellingId;

            return response;
        }

        public async Task<DbResponse> ExpenseAdd(SellingExpenseAddModel model)
        {

            var selling = await Context.Selling.FindAsync(model.SellingId);

            if (selling == null) return new DbResponse(false, $"Selling Id not found");

            if (model.Expense <= 0) return new DbResponse(false, $"ExpenseTotal must be more than zero");

            var expense = new SellingExpense
            {
                SellingId = model.SellingId,
                Expense = model.Expense,
                ExpenseDescription = model.ExpenseDescription
            };

            await Context.SellingExpense.AddAsync(expense);

            selling.ExpenseTotal += model.Expense;
            Context.Selling.Update(selling);
            await Context.SaveChangesAsync();
            return new DbResponse(true, $"Successfully expense added");
        }

        public async Task<DbResponse> ExpenseDelete(int sellingExpenseId)
        {
            var sellingExpense = await Context.SellingExpense.FindAsync(sellingExpenseId);
            var selling = await Context.Selling.FindAsync(sellingExpense.SellingId);

            if (selling == null) return new DbResponse(false, $"Selling Id not found");

            Context.SellingExpense.Remove(sellingExpense);

            selling.ExpenseTotal -= sellingExpense.Expense;
            Context.Selling.Update(selling);
            await Context.SaveChangesAsync();
            return new DbResponse(true, $"Successfully expense deleted");
        }

        public List<SellingExpenseListModel> ExpenseList(int sellingId)
        {
            return Context.SellingExpense
                .Where(e => e.SellingId == sellingId)
                .ProjectTo<SellingExpenseListModel>(_mapper.ConfigurationProvider)
                .OrderBy(a => a.InsertDateUtc)
                .ToList();
        }
    }
}