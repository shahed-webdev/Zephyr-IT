using InventoryManagement.Data;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class ExpenseCategoryRepository : Repository<ExpenseCategory>, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ICollection<DDL> ddl()
        {
            return Context.ExpenseCategory.Select(e => new DDL
            {
                value = e.ExpenseCategoryId,
                label = e.CategoryName
            }).ToList();
        }

        public bool RemoveCustom(int id)
        {
            if (Context.Expense.Any(e => e.ExpenseCategoryId == id)) return false;
            Remove(Find(id));
            return true;
        }
    }
}
