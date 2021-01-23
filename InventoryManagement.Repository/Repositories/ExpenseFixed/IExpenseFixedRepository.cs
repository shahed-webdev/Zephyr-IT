using InventoryManagement.Data;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IExpenseFixedRepository : IRepository<ExpenseFixed>
    {
        ExpenseFixed ExpenseFixed { get; set; }
        void Add(ExpenseFixedAddModel model);
        void Delete(int expenseFixedId);
        ICollection<ExpenseFixedViewModel> Records();
    }
}