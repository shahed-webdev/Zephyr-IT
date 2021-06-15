using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<int> GetNewSnAsync();
        Task<DbResponse<int>> AddCustomAsync(PurchaseViewModel model, IUnitOfWork db);
        Task<PurchaseReceiptViewModel> PurchaseReceiptAsync(int id, IUnitOfWork db);
        DataResult<PurchaseRecordViewModel> Records(DataRequest request);
        ICollection<int> Years();
        decimal TotalDue();
        decimal DailyPurchaseAmount(DateTime? date);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);
        Task<DbResponse> UpdateMemoNumberAsync(int purchaseId, string newMemoNumber);
        Task<DbResponse<PurchaseGetByReceiptModel>> GetDetailsByReceiptNo(int receipt);
        Task<PurchaseUpdateGetModel> FindUpdateBillAsync(int id, IUnitOfWork db);
        Task<DbResponse<int>> BillUpdated(PurchaseUpdatePostModel model, IUnitOfWork db, string userName);

    }
}