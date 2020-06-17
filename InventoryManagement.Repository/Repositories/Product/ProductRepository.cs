using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void AddCustom(ProductShowViewModel model)
        {
            var product = new Product
            {
                ProductCatalogId = model.ProductCatalogId,
                ProductName = model.ProductName,
                Description = model.Description,
                Warranty = model.Warranty
            };

            Add(product);
        }

        public Task<bool> IsExistAsync(string name, int categoryId, int updateId = 0)
        {
            var product = Context.Product.Where(p => p.ProductCatalogId == categoryId && p.ProductName == name);

            if (updateId != 0) product.Where(p => p.ProductId != updateId);
            return product.AnyAsync();
        }

        public Task<List<ProductShowViewModel>> FindByCategoryAsync(int categoryId, IUnitOfWork db)
        {
            return Context.Product.Where(p => p.ProductCatalogId == categoryId).Select(p => new ProductShowViewModel
            {
                ProductId = p.ProductId,
                ProductCatalogId = p.ProductCatalogId,
                ProductName = p.ProductName,
                Description = p.Description,
                Warranty = p.Warranty
            }).ToListAsync();
        }

        public bool RemoveCustom(int id)
        {
            if (Context.ProductStock.Any(e => e.ProductId == id)) return false;
            Remove(Find(id));
            return true;
        }
    }
}