using System;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IPageLinkRepository PageLinks { get; }
        IPageLinkCategoryRepository PageLinkCategories { get; }
        IPageLinkAssignRepository PageLinkAssigns { get; }
        IRegistrationRepository Registrations { get; }
        IExpenseCategoryRepository ExpenseCategories { get; }
        IExpenseRepository Expenses { get; }
        IInstitutionRepository Institutions { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
