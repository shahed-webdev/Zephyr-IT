using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;

namespace InventoryManagement.BusinessLogin
{
    public interface IAccountCore 
    {
        DbResponse<AccountCrudModel> Add(AccountCrudModel model);
        DbResponse Edit(AccountCrudModel model);
        DbResponse Delete(int id);
        DbResponse<AccountCrudModel> Get(int id);
        DataResult<AccountCrudModel> List(DataRequest request);
    }
}