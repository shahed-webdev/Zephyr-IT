using InventoryManagement.Data;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
            PageLinks = new PageLinkRepository(_context);
            PageLinkCategories = new PageLinkCategoryRepository(_context);
            PageLinkAssigns = new PageLinkAssignRepository(_context);
            ProductCatalogs = new ProductCatalogRepository(_context);
            ProductCatalogTypes = new ProductCatalogTypeRepository(_context);
            Registrations = new RegistrationRepository(_context);
            ExpenseCategories = new ExpenseCategoryRepository(_context);
            Expenses = new ExpenseRepository(_context);
            Institutions = new InstitutionRepository(_context);
            Vendors = new VendorRepository(_context);


        }


        public ICustomerRepository Customers { get; }
        public IPageLinkRepository PageLinks { get; private set; }
        public IPageLinkCategoryRepository PageLinkCategories { get; private set; }
        public IPageLinkAssignRepository PageLinkAssigns { get; private set; }
        public IProductCatalogRepository ProductCatalogs { get; }
        public IProductCatalogTypeRepository ProductCatalogTypes { get; }
        public IRegistrationRepository Registrations { get; private set; }
        public IExpenseCategoryRepository ExpenseCategories { get; private set; }
        public IExpenseRepository Expenses { get; }
        public IInstitutionRepository Institutions { get; }
        public IVendorRepository Vendors { get; }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
