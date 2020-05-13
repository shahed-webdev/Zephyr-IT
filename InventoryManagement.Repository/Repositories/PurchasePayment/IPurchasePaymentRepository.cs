using InventoryManagement.Data;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IPurchasePaymentRepository : IRepository<PurchasePayment>
    {
        Task<int> GetNewSnAsync();

    }
}