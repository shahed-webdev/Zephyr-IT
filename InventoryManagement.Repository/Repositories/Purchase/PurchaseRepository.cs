using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                PurchaseDate = model.PurchaseDate,
                Product = model.Products.Select(p => new Product
                {
                    ProductCatalogId = p.ProductCatalogId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Warranty = p.Warranty,
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    ProductStock = p.ProductStocks.Select(s => new ProductStock
                    {
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
                                PaidDate = model.PurchaseDate
                            }
                        }
                    } : null
            };

            await Context.Purchase.AddAsync(purchase).ConfigureAwait(false);
            try
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);

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
                .Include(p => p.Product)
                .ThenInclude(pd => pd.ProductCatalog)
                .Include(p => p.Product)
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
                    Products = p.Product.Select(pd => new ProductViewModel
                    {
                        ProductId = pd.ProductId,
                        ProductCatalogId = pd.ProductCatalogId,
                        ProductCatalogName = db.ProductCatalogs.CatalogNameNode(pd.ProductCatalogId),
                        ProductName = pd.ProductName,
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
                        PaymentMethod = pp.PurchasePayment.PaymentMethod,
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

        public Task<DataResult<PurchaseRecordViewModel>> RecordsAsync(DataRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}