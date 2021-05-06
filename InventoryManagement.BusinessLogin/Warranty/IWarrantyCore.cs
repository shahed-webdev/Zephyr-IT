using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;

namespace InventoryManagement.BusinessLogin
{
    public interface IWarrantyCore
    {
        DbResponse<int> Acceptance(WarrantyAcceptanceModel model, string userName);
        DbResponse<WarrantyReceiptModel> Receipt(int warrantyId);
        DbResponse<int> Delivery(WarrantyDeliveryModel model, string userName);
        DataResult<WarrantyListViewModel> List(DataRequest request);
    }
}