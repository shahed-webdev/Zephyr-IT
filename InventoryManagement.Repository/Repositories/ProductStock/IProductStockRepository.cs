using InventoryManagement.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IProductStockRepository : IRepository<ProductStock>
    {
        Task<bool> IsExistAsync(string productStock);
        Task<List<ProductStockViewModel>> IsExistListAsync(List<ProductStockViewModel> stocks);
        bool IsStockOut(string[] codes);
        bool IsInStock(string code);
        bool IsInStock(int productStockId);
        Task<ProductSellViewModel> FindforSellAsync(string code);
        Task<ProductStockDetailsViewModel> FindforDetailsAsync(string code);

        Task<DbResponse<ProductBillStockModel>> FindSellingIdAsync(string code);
        Task<List<ProductStock>> SellingStockFromCodesAsync(string[] codes);
        Task<List<ProductStock>> SoldBillStockFromCodesAsync(int sellingId, string[] codes);
        Task<List<ProductStockReportModel>> StockReport();
        decimal StockProductPurchaseValue();

        void StockOut(int productStockId);
        void StockIn(int productStockId);

    }
}