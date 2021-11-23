using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductStockRepository : Repository<ProductStock>, IProductStockRepository
    {
        public ProductStockRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<bool> IsExistAsync(string productStock)
        {
            return Context.ProductStock.AnyAsync(s => s.ProductCode == productStock);
        }

        public Task<List<ProductStockViewModel>> IsExistListAsync(List<ProductStockViewModel> stocks)
        {
            // var codes = stocks.Select(c => c.ProductCode).ToList();
            return Context.ProductStock.Where(s => stocks.Select(c => c.ProductCode).Contains(s.ProductCode)).Select(s => new ProductStockViewModel
            {
                ProductCode = s.ProductCode,
                IsSold = s.IsSold
            }).ToListAsync();
        }

        public bool IsStockOut(string[] codes)
        {
            var codeCount = codes.Length;
            var availableStockCount = Context.ProductStock.Count(s => codes.Contains(s.ProductCode) && !s.IsSold);

            return codeCount != availableStockCount;
        }

        public bool IsInStock(string code)
        {
            return Context.ProductStock.Any(s => s.ProductCode == code && !s.IsSold);
        }

        public bool IsInStock(int productStockId)
        {
            return Context.ProductStock.Any(s => s.ProductStockId == productStockId && !s.IsSold);
        }

        public Task<ProductSellViewModel> FindforSellAsync(string code)
        {
            var product = Context.ProductStock
                .Include(s => s.Product)
                .Include(s => s.PurchaseList)
                .ThenInclude(pl => pl.Purchase)
                .Where(s => s.ProductCode == code && !s.IsSold)
                .Select(s => new ProductSellViewModel
                {
                    ProductId = s.ProductId,
                    ProductStockId = s.ProductStockId,
                    ProductCatalogId = s.Product.ProductCatalogId,
                    ProductCatalogName = s.Product.ProductCatalog.CatalogName,
                    ProductCode = s.ProductCode,
                    ProductName = s.Product.ProductName,
                    Description = s.PurchaseList.Description,
                    Warranty = s.PurchaseList.Warranty,
                    Note = s.PurchaseList.Note,
                    SellingPrice = s.PurchaseList.SellingPrice,
                    PurchasePrice = s.PurchaseList.PurchasePrice - (s.PurchaseList.PurchasePrice * (s.PurchaseList.Purchase.PurchaseDiscountPercentage / 100)),
                });
            return product.FirstOrDefaultAsync();
        }

        public Task<ProductStockDetailsViewModel> FindforDetailsAsync(string code)
        {
            var product = Context.ProductStock
                .Include(s => s.Product)
                .ThenInclude(p => p.ProductCatalog)
                .Include(s => s.PurchaseList)
                .ThenInclude(pl => pl.Purchase)
                .Include(p => p.SellingList)
                .ThenInclude(sl => sl.Selling)
                .Include(s => s.ProductDamaged)
                .Where(s => s.ProductCode == code || s.Warranty.Any(w=> w.ChangedProductCode == code))
                .OrderByDescending(s => s.ProductStockId)
                .Select(s => new ProductStockDetailsViewModel
                {
                    ProductStockId = s.ProductStockId,
                    ProductId = s.ProductId,
                    ProductCode = s.ProductCode,
                    ProductName = s.Product.ProductName,
                    Description = s.PurchaseList.Description,
                    Warranty = s.PurchaseList.Warranty,
                    Note = s.PurchaseList.Note,
                    SellingPrice = s.PurchaseList.SellingPrice,
                    ProductCatalogName = s.Product.ProductCatalog.CatalogName,
                    PurchasePrice = s.PurchaseList.PurchasePrice - (s.PurchaseList.PurchasePrice * (s.PurchaseList.Purchase.PurchaseDiscountPercentage / 100)),
                    SellingId = s.SellingList.SellingId,
                    SellingSn = s.SellingList.Selling.SellingSn,
                    PurchaseId = s.PurchaseList.PurchaseId,
                    IsDamaged = s.ProductDamaged != null
                }).FirstOrDefaultAsync();

            return product;
        }

        public async Task<DbResponse<ProductBillStockModel>> FindSellingIdAsync(string code)
        {
            var productStock = await Context.ProductStock
                .Include(p => p.SellingList)
                .Where(s => s.ProductCode == code)
                .OrderByDescending(s => s.SellingList.SellingId)
                .Select(s => s).FirstOrDefaultAsync();

            if (productStock.SellingListId == null)
            {
                var warranty = await Context.Warranty.Where(w => w.ChangedProductCode == code).FirstOrDefaultAsync();

                if (warranty == null)
                    return new DbResponse<ProductBillStockModel>(false, "Selling receipt not found");

                var data = new ProductBillStockModel
                {
                    SellingId = warranty.SellingId,
                    ProductStockId = warranty.ProductStockId
                };
                return new DbResponse<ProductBillStockModel>(true, "Success", data);
            }
            var d = new ProductBillStockModel
            {
                SellingId = productStock.SellingList.SellingId,
                ProductStockId = productStock.ProductStockId
            };
            return new DbResponse<ProductBillStockModel>(true, "Success", d);
        }

        public Task<List<ProductStock>> SellingStockFromCodesAsync(string[] codes)
        {
            return Context.ProductStock.Include(s => s.Product).Where(s => codes.Contains(s.ProductCode) && !s.IsSold).ToListAsync();
        }

        public Task<List<ProductStock>> SoldBillStockFromCodesAsync(int sellingId, string[] codes)
        {
            return Context.ProductStock.Include(s => s.Product).Where(s => codes.Contains(s.ProductCode) && s.SellingList.SellingId == sellingId).ToListAsync();
        }

        public Task<List<ProductStockReportModel>> StockReport()
        {
            return Context
                .ProductCatalog
                .Where(c => c.Product.Any(p => p.ProductStock.Any(s => !s.IsSold)))
                .Select(c => new ProductStockReportModel
                {
                    ProductCatalogId = c.ProductCatalogId,
                    ProductCatalogName = c.CatalogName,
                    ProductValue = c.Product.Where(p => p.ProductStock.Any(s => !s.IsSold)).SelectMany(p => p.PurchaseList).Sum(l=> l.PurchasePrice),
                    ProductList = c.Product
                        .Where(p => p.ProductStock.Any(s => !s.IsSold))
                        .OrderBy(p => p.ProductName)
                        .Select(p => new ProductStockListModel
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            Description = p.Description,
                            Warranty = p.Warranty,
                            Note = p.Note,
                            SellingPrice = p.SellingPrice,
                            ProductCodes = p.ProductStock.Where(s => !s.IsSold).OrderBy(s => s.ProductCode).Select(s => s.ProductCode).ToArray()
                        }).ToList()
                }).OrderBy(c => c.ProductCatalogName).ToListAsync();
        }

        public decimal StockProductPurchaseValue()
        {
            return Context.ProductStock.Include(s => s.PurchaseList)
                       .Where(s => !s.IsSold)?.Sum(s => s.PurchaseList.PurchasePrice) ?? 0;
        }

        public void StockOut(int productStockId)
        {
            var stock = Context.ProductStock.Find(productStockId);
            stock.IsSold = true;
            Context.ProductStock.Update(stock);
            Context.SaveChanges();
        }

        public void StockIn(int productStockId)
        {
            var stock = Context.ProductStock.Find(productStockId);
            stock.IsSold = false;
            Context.ProductStock.Update(stock);
            Context.SaveChanges();
        }
    }
}