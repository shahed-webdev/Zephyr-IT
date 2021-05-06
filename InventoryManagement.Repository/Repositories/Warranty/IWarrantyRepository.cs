using JqueryDataTables.LoopsIT;

namespace InventoryManagement.Repository
{
    public interface IWarrantyRepository
    {
        int GetNewSn();
        DbResponse<int> Acceptance(WarrantyAcceptanceModel model);
        bool IsInWarranty(int productStockId);
        bool IsNull(int warrantyId);
        DbResponse<WarrantyAcceptanceReceiptModel> AcceptanceReceipt(int warrantyId);

        DataResult<WarrantyListViewModel> List(DataRequest request);

    }
}