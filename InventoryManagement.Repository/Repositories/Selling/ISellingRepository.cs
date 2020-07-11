using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface ISellingRepository : IRepository<Selling>
    {
        Task<int> GetNewSnAsync();
        Task<DbResponse<int>> AddCustomAsync(SellingViewModel model, IUnitOfWork db);
        Task<SellingReceiptViewModel> SellingReceiptAsync(int id, IUnitOfWork db);
        DataResult<SellingRecordViewModel> Records(DataRequest request);
        ICollection<int> Years();
        double TotalDue();
        double DailySaleAmount(DateTime? day);
        double DailySoldPurchaseAmount(DateTime? day);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);
        ICollection<MonthlyAmount> MonthlySoldPurchaseAmounts(int year);
    }
}