using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
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
            var StockOuts = await db.ProductStocks.IsStockOutAsync(model.ProductCodes).ConfigureAwait(false);
            if (StockOuts.Length > 0)
            {
                response.Message = "Product Stock Out";
                response.IsSuccess = false;
                return response;
            }

            var newSellingSn = await GetNewSnAsync().ConfigureAwait(false);
            var newSellingPaymentSn = await db.SellingPayments.GetNewSnAsync().ConfigureAwait(false);
            var sellingStock = await db.ProductStocks.SellingStockFromCodesAsync(model.ProductCodes);

            var selling = new Selling
            {
                RegistrationId = model.RegistrationId,
                CustomerId = model.CustomerId,
                SellingSn = newSellingSn,
                SellingTotalPrice = model.SellingTotalPrice,
                SellingDiscountAmount = model.SellingDiscountAmount,
                SellingDiscountPercentage = model.SellingDiscountAmount,
                SellingPaidAmount = model.SellingPaidAmount,
                SellingDate = model.SellingDate,
                SellingList = model.ProductList.Select(l => new SellingList
                {
                    ProductId = l.ProductId,
                    SellingPrice = l.SellingPrice,
                    Description = l.Description,
                    Warranty = l.Warranty,
                    ProductStock = sellingStock.Where(s => s.ProductId == l.ProductId).Select(s =>
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
                                PaidDate = model.SellingDate
                            }
                        }
                    } : null
            };


            await Context.Selling.AddAsync(selling).ConfigureAwait(false);
            try
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);

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
                  SellingDate = s.SellingDate,
                  Products = s.SellingList.Select(pd => new SellingReceiptProductViewModel
                  {
                      ProductId = pd.Product.ProductId,
                      ProductCatalogId = pd.Product.ProductCatalogId,
                      ProductCatalogName = db.ProductCatalogs.CatalogNameNode(pd.Product.ProductCatalogId),
                      ProductName = pd.Product.ProductName,
                      Description = pd.Product.Description,
                      Warranty = pd.Product.Warranty,
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
                      OrganizationName = s.Customer.OrganizationName,
                      CustomerName = s.Customer.CustomerName,
                      CustomerAddress = s.Customer.CustomerAddress,
                      CustomerPhone = s.Customer.CustomerPhone.FirstOrDefault().Phone
                  },
                  InstitutionInfo = db.Institutions.FindCustom(),
                  SoildBy = s.Registration.Name
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
    }
}