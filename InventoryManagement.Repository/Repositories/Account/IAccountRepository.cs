using System.Collections.Generic;
using JqueryDataTables.LoopsIT;

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

        void BalanceAdd(int id, decimal amount);

        void BalanceSubtract(int id, decimal amount);
    }
}