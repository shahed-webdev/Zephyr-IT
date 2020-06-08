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
                ProductCode = s.ProductCode
            }).ToListAsync();
        }

        public Task<string[]> IsStockOutAsync(string[] codes)
        {
            return Context.ProductStock
                .Where(s => codes.Contains(s.ProductCode) && s.IsSold)
                .Select(s => s.ProductCode)
                .ToArrayAsync();
        }

        public Task<ProductSellViewModel> FindforSellAsync(string code)
        {
            var product = Context.ProductStock.Include(s => s.Product).Where(s => s.ProductCode == code && !s.IsSold)
                .Select(s => new ProductSellViewModel
                {
                    ProductId = s.ProductId,
                    ProductCatalogId = s.Product.ProductCatalogId,
                    ProductCode = s.ProductCode,
                    ProductName = s.Product.ProductName,
                    Description = s.Product.Description,
                    Warranty = s.Product.Warranty,
                    SellingPrice = s.Product.SellingPrice
                });
            return product.FirstOrDefaultAsync();
        }

        public Task<List<ProductStock>> SellingStockFromCodesAsync(string[] codes)
        {
            return Context.ProductStock.Include(s => s.Product).Where(s => codes.Contains(s.ProductCode)).ToListAsync();
        }
    }
}