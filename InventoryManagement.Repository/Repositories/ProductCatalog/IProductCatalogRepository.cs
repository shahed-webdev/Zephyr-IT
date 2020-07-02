using InventoryManagement.Data;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IProductCatalogRepository : IRepository<ProductCatalog>, IAddCustomAsync<ProductCatalogViewModel>
    {
        IEnumerable<ProductCatalogShow> ListCustom();
        ICollection<DDL> CatalogDll();
        string CatalogNameNode(int id);
        void DeleteCustom(int id);
        ProductCatalogUpdateViewModel FindForUpdate(int id);
        void UpdateCustom(ProductCatalogUpdateViewModel model);
    }
}