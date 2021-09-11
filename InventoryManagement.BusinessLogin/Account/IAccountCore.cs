using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System.Collections.Generic;

namespace InventoryManagement.BusinessLogin
{
    public interface IAccountCore
    {
        DbResponse<AccountCrudModel> Add(AccountCrudModel model);
        DbResponse Edit(AccountCrudModel model);
        DbResponse Delete(int id);
        DbResponse<AccountCrudModel> Get(int id);
        List<AccountCrudModel> List();
        List<DDL> DdlList();

        DbResponse<AccountDepositCrudModel> Deposit(AccountDepositCrudModel model);
        DbResponse<AccountWithdrawCrudModel> Withdraw(AccountWithdrawCrudModel model);

        DbResponse DepositDelete(int id);
        DbResponse WithdrawDelete(int id);

        DataResult<AccountDepositCrudModel> DepositList(DataRequest request);
        DataResult<AccountWithdrawCrudModel> WithdrawList(DataRequest request);
        void DefaultAccountSet(int accountId);
        int DefaultAccountGet();
        void CapitalSet(decimal amount);
        decimal CapitalGet();
        DbResponse TransferToDefault(int accountId, decimal amount);
    }
}