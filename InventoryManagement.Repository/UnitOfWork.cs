using AutoMapper;
using InventoryManagement.Data;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            AdminMoneyCollection = new AdminMoneyCollectionRepository(_context, _mapper);
            Account = new AccountRepository(_context, _mapper);
            AccountDeposit = new AccountDepositRepository(_context, _mapper);
            AccountWithdraw = new AccountWithdrawRepository(_context, _mapper);
            Customers = new CustomerRepository(_context, _mapper);
            PageLinks = new PageLinkRepository(_context);
            PageLinkCategories = new PageLinkCategoryRepository(_context);
            PageLinkAssigns = new PageLinkAssignRepository(_context);
            Products = new ProductRepository(_context);
            ProductCatalogs = new ProductCatalogRepository(_context);
            ProductCatalogTypes = new ProductCatalogTypeRepository(_context);
            ProductDamaged = new ProductDamagedRepository(_context, _mapper);
            ProductLog = new ProductLogRepository(_context, _mapper);
            ProductStocks = new ProductStockRepository(_context);
            Purchases = new PurchaseRepository(_context);
            PurchasePayments = new PurchasePaymentRepository(_context, _mapper);
            Registrations = new RegistrationRepository(_context);
            ExpenseCategories = new ExpenseCategoryRepository(_context);
            Expenses = new ExpenseRepository(_context, _mapper);
            ExpenseFixed = new ExpenseFixedRepository(_context, mapper);
            ExpenseTransportations = new ExpenseTransportationRepository(_context, _mapper);
            Institutions = new InstitutionRepository(_context);
            Selling = new SellingRepository(_context, _mapper);
            SellingPayments = new SellingPaymentRepository(_context);
            Vendors = new VendorRepository(_context, _mapper);
            Warranty = new WarrantyRepository(_context, _mapper);

        }


        public IAdminMoneyCollectionRepository AdminMoneyCollection { get; }
        public IAccountRepository Account { get; }
        public IAccountDepositRepository AccountDeposit { get; }
        public IAccountWithdrawRepository AccountWithdraw { get; }
        public ICustomerRepository Customers { get; }
        public IPageLinkRepository PageLinks { get; private set; }
        public IPageLinkCategoryRepository PageLinkCategories { get; private set; }
        public IPageLinkAssignRepository PageLinkAssigns { get; private set; }
        public IProductRepository Products { get; }
        public IProductCatalogRepository ProductCatalogs { get; }
        public IProductCatalogTypeRepository ProductCatalogTypes { get; }
        public IProductDamagedRepository ProductDamaged { get; }
        public IProductLogRepository ProductLog { get; }
        public IProductStockRepository ProductStocks { get; }
        public IPurchaseRepository Purchases { get; }
        public IPurchasePaymentRepository PurchasePayments { get; }
        public IRegistrationRepository Registrations { get; private set; }
        public IExpenseCategoryRepository ExpenseCategories { get; private set; }
        public IExpenseRepository Expenses { get; }
        public IExpenseFixedRepository ExpenseFixed { get; }
        public IExpenseTransportationRepository ExpenseTransportations { get; }
        public IInstitutionRepository Institutions { get; }
        public IVendorRepository Vendors { get; }
        public ISellingRepository Selling { get; }
        public ISellingPaymentRepository SellingPayments { get; }
        public IWarrantyRepository Warranty { get; }


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
