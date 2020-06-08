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
                SellingList = sellingStock.Select(s => new SellingList
                {

                    ProductStockId = s.ProductStockId,
                    SellingPrice = s.Product.SellingPrice,

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
                sellingStock.ForEach(s => s.IsSold = true);
                Context.ProductStock.UpdateRange(sellingStock);

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

            return response;
        }

        public Task<SellingReceiptViewModel> PurchaseReceiptAsync(int id, IUnitOfWork db)
        {
            throw new System.NotImplementedException();
        }

        public DataResult<SellingRecordViewModel> Records(DataRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}