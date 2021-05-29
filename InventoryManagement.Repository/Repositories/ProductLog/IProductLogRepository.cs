using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IProductLogRepository
    {
        void Add(ProductLogAddModel model);
        void AddList(List<ProductLogAddModel> models);

        Task<DbResponse<List<ProductLogViewModel>>> FindLogAsync(int productStockId);
        Task<DbResponse<List<ProductLogViewModel>>> FindLogByCodeAsync(string code);
    }
}