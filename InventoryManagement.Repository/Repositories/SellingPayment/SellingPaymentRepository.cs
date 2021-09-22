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

        public CustomerMultipleDueCollectionViewModel GetSellingDuePayMultipleBill(int customerId)
        {
            var customer = Context.Customer
                .Include(c => c.Selling)
                .Where(v => v.CustomerId == customerId)
                .Select(c => new CustomerMultipleDueCollectionViewModel
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    CustomerAddress = c.CustomerAddress,
                    TotalDue = c.Due,
                    SellingDueRecords = c.Selling.Where(s => s.SellingDueAmount > 0).Select(s => new CustomerSellingViewModel
                    {
                        CustomerId = s.CustomerId,
                        SellingId = s.SellingId,
                        SellingSn = s.SellingSn,
                        SellingTotalPrice = s.SellingTotalPrice,
                        ServiceCharge = s.ServiceCharge,
                        SellingPaidAmount = s.SellingPaidAmount,
                        AccountTransactionCharge = s.AccountTransactionCharge,
                        SellingReturnAmount = s.SellingReturnAmount,
                        SellingDiscountAmount = s.SellingDiscountAmount,
                        SellingDueAmount = s.SellingDueAmount,
                        SellingDate = s.SellingDate,
                        LastUpdateDate = s.SellingDate,
                        PromisedPaymentDate = s.PromisedPaymentDate
                    }).ToList()

                });
            return customer.FirstOrDefault();
        }

        public async Task<DbResponse> DuePaySingleAsync(SellingDuePaySingleModel model, IUnitOfWork db)
        {
            var response = new DbResponse();
            try
            {
                var Sn = await db.SellingPayments.GetNewSnAsync().ConfigureAwait(false);

                var selling = await Context.Selling.FindAsync(model.SellingId).ConfigureAwait(false);

                var accountCostPercentage = db.Account.GetCostPercentage(model.AccountId.GetValueOrDefault());

                var due = (selling.SellingTotalPrice + selling.ServiceCharge + selling.SellingReturnAmount + selling.AccountTransactionCharge) -
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
                        AccountTransactionCharge = model.AccountTransactionCharge,
                        PaymentMethod = model.PaymentMethod,
                        PaidDate = DateTime.Now.BdTime().Date,
                        AccountId = model.AccountId,
                        AccountCostPercentage = accountCostPercentage,

                        SellingPaymentList = new List<SellingPaymentList>
                        {
                            new SellingPaymentList
                            {
                                SellingId = model.SellingId,
                                SellingPaidAmount = model.PaidAmount,
                                AccountTransactionCharge = model.AccountTransactionCharge
                            }
                        }
                    };
                    await Context.SellingPayment.AddAsync(sellingPayment).ConfigureAwait(false);
                }

                selling.SellingDiscountAmount = model.SellingDiscountAmount;
                selling.SellingPaidAmount += model.PaidAmount;
                selling.AccountTransactionCharge += model.AccountTransactionCharge;
                selling.SellingAccountCost += model.PaidAmount * accountCostPercentage / 100;
                selling.LastUpdateDate = DateTime.Now.BdTime().Date;

                Context.Selling.Update(selling);

                //Account add balance
                if (model.PaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceAdd(model.AccountId.Value, model.PaidAmount);

                //Sales parson add balance
                if (model.PaidAmount > 0)
                    db.Registrations.BalanceAdd(model.RegistrationId, model.PaidAmount);

                await db.SaveChangesAsync().ConfigureAwait(false);

                db.Customers.UpdatePaidDue(model.CustomerId);



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

                //Calculate Each Bill Transaction Charge
                model.CalculateEachBillTransactionCharge();

                var accountCostPercentage = db.Account.GetCostPercentage(model.AccountId.GetValueOrDefault());

                foreach (var invoice in model.Bills)
                {
                    var sell = sells.FirstOrDefault(s => s.SellingId == invoice.SellingId);

                    var due = sell.SellingDueAmount;
                    if (invoice.SellingPaidAmount - invoice.AccountTransactionCharge > due)
                    {
                        response.IsSuccess = false;
                        response.Message = $"{invoice.SellingPaidAmount} Paid amount is greater than due";
                        return response;
                    }
                    sell.SellingPaidAmount += invoice.SellingPaidAmount;
                    sell.AccountTransactionCharge += invoice.AccountTransactionCharge;
                    sell.SellingAccountCost += model.PaidAmount * accountCostPercentage / 100;
                    sell.LastUpdateDate = DateTime.Now.BdTime().Date;
                }

                var Sn = await db.SellingPayments.GetNewSnAsync().ConfigureAwait(false);

                var receipt = new SellingPayment
                {
                    RegistrationId = model.RegistrationId,
                    CustomerId = model.CustomerId,
                    ReceiptSn = Sn,
                    PaidAmount = model.PaidAmount,
                    AccountTransactionCharge = model.AccountTransactionCharge,
                    AccountId = model.AccountId,
                    AccountCostPercentage = accountCostPercentage,
                    PaidDate = DateTime.Now.BdTime().Date,
                    SellingPaymentList = model.Bills.Select(i => new SellingPaymentList
                    {
                        SellingId = i.SellingId,
                        SellingPaidAmount = i.SellingPaidAmount,
                        AccountTransactionCharge = i.AccountTransactionCharge
                    }).ToList()
                };

                await Context.SellingPayment.AddAsync(receipt).ConfigureAwait(false);
                Context.Selling.UpdateRange(sells);

                //Account add balance
                if (model.PaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceAdd(model.AccountId.Value, model.PaidAmount);

                //Sales parson add balance
                if (model.PaidAmount > 0)
                    db.Registrations.BalanceAdd(model.RegistrationId, model.PaidAmount);

                await db.SaveChangesAsync().ConfigureAwait(false);

                db.Customers.UpdatePaidDue(model.CustomerId);


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
                .Include(s => s.Account)
                .Include(s => s.Customer)
                .Include(s => s.Registration)
                .Select(s => new SellingPaymentRecordModel
                {
                    SellingPaymentId = s.SellingPaymentId,
                    CustomerId = s.CustomerId,
                    RegistrationId = s.RegistrationId,
                    CollectBy = $" {s.Registration.Name} ({s.Registration.UserName})",
                    CustomerName = s.Customer.CustomerName,
                    ReceiptSn = s.ReceiptSn,
                    SellingSn = s.SellingPaymentList.Select(p => p.Selling.SellingSn).FirstOrDefault(),
                    SellingId = s.SellingPaymentList.Select(p => p.Selling.SellingId).FirstOrDefault(),
                    PaidAmount = s.PaidAmount,
                    PaymentMethod = s.Account.AccountName,
                    PaidDate = s.PaidDate
                })
                .ToDataResult(request);
        }

        public decimal Capital(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 12, 31);

            return Context
                .SellingPaymentList
                .Include(p => p.SellingPayment)
                .Include(p => p.Selling)
                .Where(r => r.SellingPayment.PaidDate <= eD && r.SellingPayment.PaidDate >= sD)
                .Sum(s => s.Selling.SellingPaidAmount > (s.Selling.BuyingTotalPrice + s.Selling.ServiceCost) ? (s.Selling.BuyingTotalPrice + s.Selling.ServiceCost) - (s.Selling.SellingPaidAmount - s.SellingPaidAmount) > 0 ? (s.Selling.BuyingTotalPrice + s.Selling.ServiceCost) - (s.Selling.SellingPaidAmount - s.SellingPaidAmount) : 0 : s.SellingPaidAmount);
        }

        public decimal CollectionAmountDateWise(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 12, 31);
            return Context
                .SellingPayment
                .Where(r => r.PaidDate <= eD && r.PaidDate >= sD).Sum(s => s.PaidAmount);
        }

        public decimal CollectionAmountDateWise(DateTime? sDateTime, DateTime? eDateTime, int registrationId)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 12, 31);
            return Context
                .SellingPayment
                .Where(r => r.PaidDate <= eD && r.PaidDate >= sD && r.RegistrationId == registrationId)
                .Sum(s => s.PaidAmount);
        }

        public List<PaymentCollectionByAccount> CollectionAccountWise(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 1, 1);
            var eD = eDateTime ?? new DateTime(DateTime.Now.BdTime().Year, 12, 31);
            var payments = Context
                 .SellingPayment
                 .Include(s => s.Account)
                 .Where(r => r.PaidDate <= eD && r.PaidDate >= sD).ToList();

            return payments.GroupBy(e => new
            {
                AccountId = e.AccountId,
                AccountName = e.Account?.AccountName
            })
                  .Select(g => new PaymentCollectionByAccount
                  {
                      AccountId = g.Key.AccountId,
                      AccountName = g.Key.AccountName,
                      Amount = g.Sum(e => e.PaidAmount)
                  })
                  .ToList();
        }
    }
}