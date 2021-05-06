using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;

namespace InventoryManagement.BusinessLogin
{
    public interface IWarrantyCore
    {
        DbResponse<int> Acceptance(WarrantyAcceptanceModel model, string userName);
        DbResponse<WarrantyAcceptanceReceiptModel> AcceptanceReceipt(int warrantyId);

        DataResult<WarrantyListViewModel> List(DataRequest request);
    }
}