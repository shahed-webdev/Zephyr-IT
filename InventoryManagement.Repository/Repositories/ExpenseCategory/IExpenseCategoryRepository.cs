using InventoryManagement.Data;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IExpenseCategoryRepository : IRepository<ExpenseCategory>
    {
        ICollection<DDL> ddl();
        bool RemoveCustom(int id);
    }
}
