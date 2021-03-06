using JqueryDataTables.LoopsIT;

namespace InventoryManagement.Repository
{
    public interface IAccountWithdrawRepository
    {

        DbResponse<AccountWithdrawCrudModel> Add(AccountWithdrawCrudModel model);
        DbResponse Delete(int id);
        bool IsNull(int id);
        DataResult<AccountWithdrawCrudModel> List(DataRequest request);

        DbResponse<AccountWithdrawCrudModel> Get(int id);
    }
}