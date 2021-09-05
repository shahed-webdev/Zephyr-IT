using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            MonthlySummary = new HashSet<MonthlyDashboardSummaryViewModel>();
            TopDueCustomer = new HashSet<CustomerDueViewModel>();
            DueVendors = new HashSet<VendorPaidDue>();
        }
        public DailyDashboardSummaryViewModel DailySummary { get; set; }
        public CapitalReportModel CapitalReport { get; set; }
        public decimal MarketDue { get; set; }
        public decimal CustomerDue { get; set; }
        public decimal StockProductPurchaseValue { get; set; }
        public ICollection<MonthlyDashboardSummaryViewModel> MonthlySummary { get; set; }
        public ICollection<CustomerDueViewModel> TopDueCustomer { get; set; }
        public ICollection<VendorPaidDue> DueVendors { get; set; }
    }

    public class MonthlyDashboardSummaryViewModel
    {
        public MonthName Month { get; set; }
        public decimal MonthlySale { get; set; }
        public decimal MonthlyNewPurchase { get; set; }
        public decimal MonthlyExpense { get; set; }
        public decimal MonthlyProfit { get; set; }
        public decimal DailyAverageProfit { get; set; }
    }

    public class DailyDashboardSummaryViewModel
    {
        public decimal TotalSale { get; set; }
        public decimal ProductSold { get; set; }
        public decimal CashCollection { get; set; }
        public decimal Expense { get; set; }
        public decimal Profit { get; set; }
        public decimal NetProfit { get; set; }
    }
}
