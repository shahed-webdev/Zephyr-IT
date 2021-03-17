using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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
        decimal TotalDue();
        decimal DailySaleAmount(DateTime? day);
        decimal SaleAmountDateWise(DateTime? sDateTime, DateTime? eDateTime);
        decimal DailyProductSoldAmount(DateTime? day);
        DbResponse<DateWiseSaleSummary> ProductSoldAmountDateWise(DateTime? fromDate, DateTime? toDate);
        decimal DailyProfit(DateTime? day);
        decimal DailySoldPurchaseAmount(DateTime? day);
        ICollection<MonthlyAmount> MonthlyAmounts(int year);
        ICollection<MonthlyAmount> MonthlyProfit(int year);
        Task<DbResponse> DeleteBillAsync(int id, IUnitOfWork db);
        Task<SellingUpdateGetModel> FindUpdateBillAsync(int id, IUnitOfWork db);
        Task<DbResponse> BillUpdated(SellingUpdatePostModel model, IUnitOfWork db);

        Task<DbResponse> ExpenseAdd(SellingExpenseAddModel model);
        Task<DbResponse> ExpenseDelete(int sellingExpenseId);

        List<SellingExpenseListModel> ExpenseList(int sellingId);
    }


}