using InventoryManagement.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IProductCatalogTypeRepository : IRepository<ProductCatalogType>, IAddCustomAsync<ProductCatalogTypeViewModel>
    {
        Task<List<ProductCatalogTypeViewModel>> ListCustomAsync();
        Task DeleteCustomAsync(int id);
        Task UpdateCustomAsync(ProductCatalogTypeViewModel model);
    }
}