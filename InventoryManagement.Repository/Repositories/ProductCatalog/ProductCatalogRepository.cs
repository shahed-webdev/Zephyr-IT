using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class ProductCatalogRepository : Repository<ProductCatalog>, IProductCatalogRepository
    {
        public ProductCatalogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
