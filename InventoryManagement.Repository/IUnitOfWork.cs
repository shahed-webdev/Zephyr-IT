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
        IProductCatalogRepository ProductCatalogs { get; }
        IProductCatalogTypeRepository ProductCatalogTypes { get; }
        IRegistrationRepository Registrations { get; }
        IExpenseCategoryRepository ExpenseCategories { get; }
        IExpenseRepository Expenses { get; }
        IInstitutionRepository Institutions { get; }
        IVendorRepository Vendors { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
