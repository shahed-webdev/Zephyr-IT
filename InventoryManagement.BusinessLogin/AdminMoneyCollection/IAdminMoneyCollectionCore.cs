using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;

namespace InventoryManagement.BusinessLogin
{
    public interface IAdminMoneyCollectionCore
    {
        DbResponse<AdminMoneyCollectionViewModel> Add(AdminMoneyCollectionAddModel model);
        DbResponse Delete(int id);
        DataResult<AdminMoneyCollectionViewModel> List(DataRequest request);
    }
}