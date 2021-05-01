using InventoryManagement.Repository;

namespace InventoryManagement.BusinessLogin
{
    public interface IWarrantyCore
    {
        DbResponse<int> Acceptance(WarrantyAcceptanceModel model, string userName);
        DbResponse<WarrantyAcceptanceReceiptModel> AcceptanceReceipt(int warrantyId);
    }
}