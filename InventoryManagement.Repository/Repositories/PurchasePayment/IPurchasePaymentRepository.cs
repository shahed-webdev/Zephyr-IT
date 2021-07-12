using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IPurchasePaymentRepository : IRepository<PurchasePayment>
    {
        Task<int> GetNewSnAsync();
        Task<DbResponse> DuePaySingleAsync(PurchaseDuePaySingleModel model, IUnitOfWork db);
        Task<DbResponse<int>> DuePayMultipleAsync(PurchaseDuePayMultipleModel model, IUnitOfWork db);
        VendorMultipleDueCollectionViewModel GetPurchaseDuePayMultipleBill(int vendorId);
        DataResult<PurchasePaymentRecordViewModel> Records(DataRequest request);
    }


}