using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<int> GetNewSnAsync();
        Task<DbResponse<int>> AddCustomAsync(PurchaseViewModel model, IUnitOfWork db);
        Task<PurchaseReceiptViewModel> PurchaseReceiptAsync(int id, IUnitOfWork db);
        DataResult<PurchaseRecordViewModel> Records(DataRequest request);
    }
}