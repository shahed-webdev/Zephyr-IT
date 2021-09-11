using InventoryManagement.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<AdminMoneyCollection> AdminMoneyCollection { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountDeposit> AccountDeposit { get; set; }
        public virtual DbSet<AccountWithdraw> AccountWithdraw { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerPhone> CustomerPhone { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseFixed> ExpenseFixed { get; set; }
        public virtual DbSet<ExpenseTransportation> ExpenseTransportation { get; set; }
        public virtual DbSet<ExpenseTransportationList> ExpenseTransportationList { get; set; }
        public virtual DbSet<ExpenseCategory> ExpenseCategory { get; set; }
        public virtual DbSet<Institution> Institution { get; set; }
        public virtual DbSet<PageLink> PageLink { get; set; }
        public virtual DbSet<PageLinkAssign> PageLinkAssign { get; set; }
        public virtual DbSet<PageLinkCategory> PageLinkCategory { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCatalog> ProductCatalog { get; set; }
        public virtual DbSet<ProductCatalogType> ProductCatalogType { get; set; }
        public virtual DbSet<ProductDamaged> ProductDamaged { get; set; }
        public virtual DbSet<ProductLog> ProductLog { get; set; }
        public virtual DbSet<ProductStock> ProductStock { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<PurchaseList> PurchaseList { get; set; }
        public virtual DbSet<PurchasePayment> PurchasePayment { get; set; }
        public virtual DbSet<PurchasePaymentList> PurchasePaymentList { get; set; }
        public virtual DbSet<PurchasePaymentReturnRecord> PurchasePaymentReturnRecord { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        public virtual DbSet<Selling> Selling { get; set; }
        public virtual DbSet<SellingExpense> SellingExpense { get; set; }
        public virtual DbSet<SellingAdjustment> SellingAdjustment { get; set; }
        public virtual DbSet<SellingList> SellingList { get; set; }
        public virtual DbSet<SellingPayment> SellingPayment { get; set; }
        public virtual DbSet<SellingPaymentList> SellingPaymentList { get; set; }
        public virtual DbSet<SellingPaymentReturnRecord> SellingPaymentReturnRecord { get; set; }
        public virtual DbSet<SellingPromiseDateMiss> SellingPromiseDateMiss { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServiceDevice> ServiceDevice { get; set; }
        public virtual DbSet<ServiceList> ServiceList { get; set; }
        public virtual DbSet<ServicePaymentList> ServicePaymentList { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<Warranty> Warranty { get; set; }
        public virtual DbSet<VW_ExpenseWithTransportation> VW_ExpenseWithTransportation { get; set; }
        public virtual DbSet<VW_CapitalProfitReport> VW_CapitalProfitReport { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AdminMoneyCollectionConfiguration());
            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new AccountDepositConfiguration());
            builder.ApplyConfiguration(new AccountWithdrawConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new CustomerPhoneConfiguration());
            builder.ApplyConfiguration(new ExpenseConfiguration());
            builder.ApplyConfiguration(new ExpenseFixedConfiguration());
            builder.ApplyConfiguration(new ExpenseCategoryConfiguration());
            builder.ApplyConfiguration(new ExpenseTransportationConfiguration());
            builder.ApplyConfiguration(new ExpenseTransportationListConfiguration());
            builder.ApplyConfiguration(new InstitutionConfiguration());
            builder.ApplyConfiguration(new PageLinkConfiguration());
            builder.ApplyConfiguration(new PageLinkAssignConfiguration());
            builder.ApplyConfiguration(new PageLinkCategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ProductCatalogConfiguration());
            builder.ApplyConfiguration(new ProductCatalogTypeConfiguration());
            builder.ApplyConfiguration(new ProductDamagedConfiguration());
            builder.ApplyConfiguration(new ProductLogConfiguration());
            builder.ApplyConfiguration(new ProductStockConfiguration());
            builder.ApplyConfiguration(new PurchaseConfiguration());
            builder.ApplyConfiguration(new PurchaseListConfiguration());
            builder.ApplyConfiguration(new PurchasePaymentConfiguration());
            builder.ApplyConfiguration(new PurchasePaymentListConfiguration());
            builder.ApplyConfiguration(new PurchasePaymentReturnRecordConfiguration());
            builder.ApplyConfiguration(new RegistrationConfiguration());
            builder.ApplyConfiguration(new SellingConfiguration());
            builder.ApplyConfiguration(new SellingExpenseConfiguration());
            builder.ApplyConfiguration(new SellingAdjustmentConfiguration());
            builder.ApplyConfiguration(new SellingListConfiguration());
            builder.ApplyConfiguration(new SellingPaymentConfiguration());
            builder.ApplyConfiguration(new SellingPaymentListConfiguration());
            builder.ApplyConfiguration(new SellingPaymentReturnRecordConfiguration());
            builder.ApplyConfiguration(new SellingPromiseDateChangeConfiguration());
            builder.ApplyConfiguration(new ServiceConfiguration());
            builder.ApplyConfiguration(new ServiceDeviceConfiguration());
            builder.ApplyConfiguration(new ServiceListConfiguration());
            builder.ApplyConfiguration(new ServicePaymentListConfiguration());
            builder.ApplyConfiguration(new VendorConfiguration());
            builder.ApplyConfiguration(new WarrantyConfiguration());
            builder.ApplyConfiguration(new VW_ExpenseWithTransportationConfiguration());
            builder.ApplyConfiguration(new VW_CapitalProfitReportConfiguration());


            base.OnModelCreating(builder);
            builder.SeedInsitutionData();
            builder.SeedAdminData();
            //builder.SeedSubAdminLinkData();
        }
    }
}