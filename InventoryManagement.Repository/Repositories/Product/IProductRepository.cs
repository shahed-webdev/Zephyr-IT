using InventoryManagement.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IProductRepository : IRepository<Product>, IAddCustom<ProductShowViewModel>
    {
        Task<bool> IsExistAsync(string name, int categoryId, int updateId = 0);
        Task<List<ProductShowViewModel>> FindByCategoryAsync(int categoryId = 0);
        bool RemoveCustom(int id);
    }
}