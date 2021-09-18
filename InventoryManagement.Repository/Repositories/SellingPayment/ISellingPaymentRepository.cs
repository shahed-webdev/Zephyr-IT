using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface ISellingPaymentRepository : IRepository<SellingPayment>
    {
        Task<int> GetNewSnAsync();
        CustomerMultipleDueCollectionViewModel GetSellingDuePayMultipleBill(int customerId);
        Task<DbResponse> DuePaySingleAsync(SellingDuePaySingleModel model, IUnitOfWork db);
        Task<DbResponse<int>> DuePayMultipleAsync(SellingDuePayMultipleModel model, IUnitOfWork db);
        decimal DailyCashCollectionAmount(DateTime? date);
        DataResult<SellingPaymentRecordModel> Records(DataRequest request);
        decimal Capital(DateTime? sDateTime, DateTime? eDateTime);
        decimal CollectionAmountDateWise(DateTime? sDateTime, DateTime? eDateTime);
        decimal CollectionAmountDateWise(DateTime? sDateTime, DateTime? eDateTime, int registrationId);
        List<PaymentCollectionByAccount> CollectionAccountWise(DateTime? sDateTime, DateTime? eDateTime);
    }
}