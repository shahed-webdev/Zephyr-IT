using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class SellingPaymentRepository : Repository<SellingPayment>, ISellingPaymentRepository
    {
        public SellingPaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetNewSnAsync()
        {
            var sn = 0;
            if (await Context.SellingPayment.AnyAsync().ConfigureAwait(false))
            {
                sn = await Context.SellingPayment.MaxAsync(p => p == null ? 0 : p.ReceiptSn).ConfigureAwait(false);
            }

            return sn + 1;
        }

        public async Task<DbResponse> DuePaySingleAsync(SellingDuePaySingleModel model, IUnitOfWork db)
        {
            var response = new DbResponse();
            try
            {
                var Sn = await db.SellingPayments.GetNewSnAsync().ConfigureAwait(false);

                var selling = await Context.Selling.FindAsync(model.SellingId).ConfigureAwait(false);



                var due = (selling.SellingTotalPrice + selling.ServiceCharge + selling.SellingReturnAmount) -
                    (model.SellingDiscountAmount + selling.SellingPaidAmount);
                if (model.PaidAmount > due)
                {
                    response.IsSuccess = false;
                    response.Message = "Paid amount is greater than due";
                }

                if (model.PaidAmount > 0)
                {
                    var sellingPayment = new SellingPayment
                    {
                        RegistrationId = model.RegistrationId,
                        CustomerId = model.CustomerId,
                        ReceiptSn = Sn,
                        PaidAmount = model.PaidAmount,
                        PaymentMethod = model.PaymentMethod,
                        PaidDate = DateTime.Now.BdTime().Date,
                        AccountId = model.AccountId,

                        SellingPaymentList = new List<SellingPaymentList>
                        {
                            new SellingPaymentList
                            {
                                SellingId = model.SellingId,
                                SellingPaidAmount = model.PaidAmount
                            }
                        }
                    };
                    await Context.SellingPayment.AddAsync(sellingPayment).ConfigureAwait(false);
                }

                selling.SellingDiscountAmount = model.SellingDiscountAmount;
                selling.SellingPaidAmount += model.PaidAmount;
                selling.LastUpdateDate = DateTime.Now.BdTime().Date;

                Context.Selling.Update(selling);

                await db.SaveChangesAsync().ConfigureAwait(false);

                db.Customers.UpdatePaidDue(model.CustomerId);

                //Account add balance
                if (model.PaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceAdd(model.AccountId.Value, model.PaidAmount);

                response.IsSuccess = true;
                response.Message = "Success";
                return response;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<DbResponse<int>> DuePayMultipleAsync(SellingDuePayMultipleModel model, IUnitOfWork db)
        {
            var response = new DbResponse<int>();
            try
            {
                var sells = await Context.Selling.Where(s => model.Bills.Select(i => i.SellingId).Contains(s.SellingId)).ToListAsync().ConfigureAwait(false);

                foreach (var invoice in model.Bills)
                {
                    var sell = sells.FirstOrDefault(s => s.SellingId == invoice.SellingId);
                    sell.SellingDiscountAmount = invoice.SellingDiscountAmount;
                    var due = (sell.SellingTotalPrice + sell.ServiceCharge + sell.SellingReturnAmount) - (invoice.SellingDiscountAmount + sell.SellingPaidAmount);
                    if (due < invoice.SellingPaidAmount)
                    {
                        response.IsSuccess = false;
                        response.Message = $"{invoice.SellingPaidAmount} Paid amount is greater than due";
                        return response;
                    }
                    sell.SellingPaidAmount += invoice.SellingPaidAmount;
                    sell.LastUpdateDate = DateTime.Now.BdTime().Date;
                }

                var Sn = await db.SellingPayments.GetNewSnAsync().ConfigureAwait(false);

                var receipt = new SellingPayment
                {
                    RegistrationId = model.RegistrationId,
                    CustomerId = model.CustomerId,
                    ReceiptSn = Sn,
                    PaidAmount = model.PaidAmount,
                    AccountId = model.AccountId,
                    PaymentMethod = model.PaymentMethod,
                    PaidDate = DateTime.Now.BdTime().Date,
                    SellingPaymentList = model.Bills.Select(i => new SellingPaymentList
                    {
                        SellingId = i.SellingId,
                        SellingPaidAmount = i.SellingPaidAmount,
                    }).ToList()
                };

                Context.SellingPayment.Add(receipt);
                Context.Selling.UpdateRange(sells);

                await db.SaveChangesAsync().ConfigureAwait(false);

                db.Customers.UpdatePaidDue(model.CustomerId);
                //Account add balance
                if (model.PaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceAdd(model.AccountId.Value, model.PaidAmount);

                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = receipt.SellingPaymentId;
                return response;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public decimal DailyCashCollectionAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now.BdTime().Date;
            return Context.SellingPayment.Where(s => s.PaidDate == saleDate.Date)?
                       .Sum(s => s.PaidAmount) ?? 0;
        }

        public DataResult<SellingPaymentRecordModel> Records(DataRequest request)
        {
            return Context.SellingPayment
                .Include(s => s.SellingPaymentList)
                .Include(s => s.Customer)
                .Select(s => new SellingPaymentRecordModel
                {
                    SellingPaymentId = s.SellingPaymentId,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.CustomerName,
                    ReceiptSn = s.ReceiptSn,
                    SellingSn = s.SellingPaymentList.Select(p => p.Selling.SellingSn).FirstOrDefault(),
                    SellingId = s.SellingPaymentList.Select(p => p.Selling.SellingId).FirstOrDefault(),
                    PaidAmount = s.PaidAmount,
                    PaymentMethod = s.PaymentMethod,
                    PaidDate = s.PaidDate
                })
                .ToDataResult(request);
        }

        public decimal CollectionAmountDateWise(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 12, 31);
            return Context
                .SellingPayment
                .Where(r => r.PaidDate <= eD && r.PaidDate >= sD).Sum(s => s.PaidAmount);
        }
    }
}