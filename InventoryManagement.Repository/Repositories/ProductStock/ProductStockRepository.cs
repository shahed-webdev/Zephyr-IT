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

    }
}