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
        public double MarketDue { get; set; }
        public double CustomerDue { get; set; }
        public double StockProductPurchaseValue { get; set; }
        public ICollection<MonthlyDashboardSummaryViewModel> MonthlySummary { get; set; }
        public ICollection<CustomerDueViewModel> TopDueCustomer { get; set; }
        public ICollection<VendorPaidDue> DueVendors { get; set; }
    }

    public class MonthlyDashboardSummaryViewModel
    {
        public MonthName Month { get; set; }
        public double MonthlySale { get; set; }
        public double MonthlyNewPurchase { get; set; }
        public double MonthlyExpense { get; set; }
        public double MonthlyProfit { get; set; }
        public double DailyAverageProfit { get; set; }
    }

    public class DailyDashboardSummaryViewModel
    {
        public double Sale { get; set; }
        public double Purchase { get; set; }
        public double Profit { get; set; }
        public double Expense { get; set; }
    }

}
