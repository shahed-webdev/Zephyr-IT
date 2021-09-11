using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IAccountRepository
    {
        DbResponse<AccountCrudModel> Add(AccountCrudModel model);
        DbResponse Edit(AccountCrudModel model);
        DbResponse Delete(int id);
        DbResponse<AccountCrudModel> Get(int id);
        bool IsExistName(string name);
        bool IsExistName(string name, int updateId);
        bool IsNull(int id);
        bool IsRelatedDataExist(int id);
        List<AccountCrudModel> List();
        List<DDL> DdlList();

        void BalanceAdd(int id, decimal amount);

        void BalanceSubtract(int id, decimal amount);
        decimal GetCostPercentage(int id);
        void DefaultAccountSet(int accountId);
        int DefaultAccountGet();

        void CapitalSet(decimal amount);
        decimal CapitalGet();

        void SellingReturnRecordAdd(SellingPaymentReturnRecordModel model);
        void PurchaseReturnRecordAdd(PurchasePaymentReturnRecordModel model);
        DbResponse TransferToDefault(int accountId, decimal amount);
    }
}