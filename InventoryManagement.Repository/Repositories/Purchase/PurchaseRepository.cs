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

            if (duplicateStocks.Count > 0)
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
                    PurchaseDate = p.PurchaseDate,
                    MemoNumber = p.MemoNumber,
                    Products = p.PurchaseList.Select(pd => new ProductViewModel
                    {
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
            var r = Context.Purchase.Include(p => p.Vendor).Select(p => new PurchaseRecordViewModel
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
            });
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
    }
}