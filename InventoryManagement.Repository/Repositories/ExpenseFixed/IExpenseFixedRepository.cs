using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;

namespace InventoryManagement.Repository
{
    public interface IExpenseFixedRepository : IRepository<ExpenseFixed>
    {
        void Add(ExpenseFixedAddModel model);
        void Delete(int expenseFixedId);
        DataResult<ExpenseFixedViewModel> RecordDataTable(DataRequest request);
    }
}