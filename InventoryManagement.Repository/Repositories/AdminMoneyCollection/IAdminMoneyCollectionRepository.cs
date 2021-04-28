using JqueryDataTables.LoopsIT;

namespace InventoryManagement.Repository
{
    public interface IAdminMoneyCollectionRepository
    {
        DbResponse<AdminMoneyCollectionViewModel> Add(AdminMoneyCollectionAddModel model);
        DbResponse Delete(int id);
        bool IsNull(int id);
        DataResult<AdminMoneyCollectionViewModel> List(DataRequest request);

        AdminMoneyCollectionViewModel Get(int id);
    }
}