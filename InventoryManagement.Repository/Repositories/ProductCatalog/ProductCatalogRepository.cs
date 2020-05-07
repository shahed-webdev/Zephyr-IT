using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductCatalogRepository : Repository<ProductCatalog>, IProductCatalogRepository
    {
        public ProductCatalogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<DbResponse<ProductCatalogViewModel>> AddCustomAsync(ProductCatalogViewModel model)
        {
            var catalog = new ProductCatalog
            {
                CatalogTypeId = model.CatalogTypeId,
                CatalogName = model.CatalogName,
                CatalogLevel = 0,
                ParentId = model.ParentId
            };

            var response = new DbResponse<ProductCatalogViewModel>();

            await Context.ProductCatalog.AddAsync(catalog).ConfigureAwait(false);

            try
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);
                model.ProductCatalogId = catalog.ProductCatalogId;
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = model;
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
