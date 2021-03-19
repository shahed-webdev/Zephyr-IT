using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class PurchasePaymentRepository : Repository<PurchasePayment>, IPurchasePaymentRepository
    {
        public PurchasePaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetNewSnAsync()
        {
            var sn = 0;
            if (await Context.PurchasePayment.AnyAsync().ConfigureAwait(false))
            {
                sn = await Context.PurchasePayment.MaxAsync(p => p == null ? 0 : p.ReceiptSn).ConfigureAwait(false);
            }

            return sn + 1;
        }

        public async Task<DbResponse> DuePaySingleAsync(PurchaseDuePaySingleModel model, IUnitOfWork db)
        {
            var response = new DbResponse();
            try
            {
                var Sn = await db.PurchasePayments.GetNewSnAsync().ConfigureAwait(false);

                var purchase = await Context.Purchase.FindAsync(model.PurchaseId).ConfigureAwait(false);



                var due = (purchase.PurchaseTotalPrice + purchase.PurchaseReturnAmount) -
                    (purchase.PurchaseDiscountAmount + purchase.PurchasePaidAmount);
                if (model.PaidAmount > due)
                {
                    response.IsSuccess = false;
                    response.Message = "Paid amount is greater than due";
                }

                if (model.PaidAmount > 0)
                {
                    var PurchasePayment = new PurchasePayment
                    {
                        RegistrationId = model.RegistrationId,
                        VendorId = model.VendorId,
                        ReceiptSn = Sn,
                        PaidAmount = model.PaidAmount,
                        PaymentMethod = model.PaymentMethod,
                        PaidDate = model.PaidDate.BdTime().Date,
                        AccountId = model.AccountId,

                        PurchasePaymentList = new List<PurchasePaymentList>
                        {
                            new PurchasePaymentList
                            {
                                PurchaseId = model.PurchaseId,
                                PurchasePaidAmount = model.PaidAmount
                            }
                        }
                    };
                    await Context.PurchasePayment.AddAsync(PurchasePayment).ConfigureAwait(false);
                }

                purchase.PurchasePaidAmount += model.PaidAmount;

                Context.Purchase.Update(purchase);

                //Account substract balance
                if (model.PaidAmount > 0 && model.AccountId != null)
                    db.Account.BalanceSubtract(model.AccountId.Value, model.PaidAmount);

                await db.SaveChangesAsync().ConfigureAwait(false);

                db.Vendors.UpdatePaidDue(model.VendorId);

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
    }
}