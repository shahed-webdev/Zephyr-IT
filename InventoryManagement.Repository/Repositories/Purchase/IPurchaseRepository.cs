using InventoryManagement.Data;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<int> GetNewSnAsync();
        Task<DbResponse<int>> AddCustomAsync(PurchaseViewModel model, IUnitOfWork db);
        Task<PurchaseReceiptViewModel> PurchaseReceiptAsync(int id);
    }
}