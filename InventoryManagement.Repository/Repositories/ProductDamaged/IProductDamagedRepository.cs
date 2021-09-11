using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IProductDamagedRepository
    {
        DbResponse Add(ProductDamagedAddModel model);
        DbResponse Delete(int productDamagedId);
        int GetProductStockId(int productDamagedId);
        DataResult<ProductDamagedViewModel> List(DataRequest request);
        decimal DamagedAmountDateWise(DateTime? fromDate, DateTime? toDate);
        ICollection<MonthlyAmount> MonthlyDamaged(int year);
    }
}