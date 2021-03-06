using JqueryDataTables.LoopsIT;

namespace InventoryManagement.Repository
{
    public interface IAccountDepositRepository
    {
        DbResponse<AccountDepositCrudModel> Add(AccountDepositCrudModel model);
        DbResponse Delete(int id);
        bool IsNull(int id);
        DataResult<AccountDepositCrudModel> List(DataRequest request);
        DbResponse<AccountDepositCrudModel> Get(int id);
    }
}