using JqueryDataTables.LoopsIT;

namespace InventoryManagement.Repository
{
    public interface IWarrantyRepository
    {
        int GetNewSn();
        DbResponse<int> Acceptance(WarrantyAcceptanceModel model);
        DbResponse<int> Delivery(WarrantyDeliveryModel model);
        bool IsInWarranty(int productStockId);
        bool IsNull(int warrantyId);
        DbResponse<WarrantyReceiptModel> Receipt(int warrantyId);
        DataResult<WarrantyListViewModel> List(DataRequest request);

    }
}