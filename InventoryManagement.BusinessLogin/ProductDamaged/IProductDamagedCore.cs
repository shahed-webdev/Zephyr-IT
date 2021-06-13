using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System;

namespace InventoryManagement.BusinessLogin
{
    public interface IProductDamagedCore
    {
        DbResponse Add(ProductDamagedAddModel model, string userName);
        DbResponse Delete(int productDamagedId, string userName);
        DataResult<ProductDamagedViewModel> List(DataRequest request);
        decimal DamagedAmountDateWise(DateTime? fromDate, DateTime? toDate);
    }
}