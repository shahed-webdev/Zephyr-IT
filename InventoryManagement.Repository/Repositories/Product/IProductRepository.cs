using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IProductRepository : IRepository<Product>, IAddCustom<ProductShowViewModel>
    {
        Task<bool> IsExistAsync(string name, int categoryId, int updateId = 0);
        Task<List<ProductShowViewModel>> FindByCategoryAsync(int categoryId = 0);
        DataResult<ProductShowViewModel> FindDataTable(DataRequest request);
        bool RemoveCustom(int id);
        Task<ProductShowViewModel> FindByIdAsync(int productId);
        DbResponse<ProductViewModel> ProductWithCodes(int productId);

        int GetLastPurchaseId(int productId);
    }
}