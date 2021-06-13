using System;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminMoneyCollectionRepository AdminMoneyCollection { get; }
        IAccountRepository Account { get; }
        IAccountDepositRepository AccountDeposit { get; }
        IAccountWithdrawRepository AccountWithdraw { get; }
        ICustomerRepository Customers { get; }
        IPageLinkRepository PageLinks { get; }
        IPageLinkCategoryRepository PageLinkCategories { get; }
        IPageLinkAssignRepository PageLinkAssigns { get; }
        IProductRepository Products { get; }
        IProductCatalogRepository ProductCatalogs { get; }
        IProductCatalogTypeRepository ProductCatalogTypes { get; }
        IProductDamagedRepository ProductDamaged { get; }
        IProductLogRepository ProductLog { get; }
        IProductStockRepository ProductStocks { get; }
        IPurchaseRepository Purchases { get; }
        IPurchasePaymentRepository PurchasePayments { get; }
        IRegistrationRepository Registrations { get; }
        IExpenseCategoryRepository ExpenseCategories { get; }
        IExpenseRepository Expenses { get; }
        IExpenseFixedRepository ExpenseFixed { get; }
        IExpenseTransportationRepository ExpenseTransportations { get; }
        IInstitutionRepository Institutions { get; }
        IVendorRepository Vendors { get; }
        ISellingRepository Selling { get; }
        ISellingPaymentRepository SellingPayments { get; }
        IWarrantyRepository Warranty { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
