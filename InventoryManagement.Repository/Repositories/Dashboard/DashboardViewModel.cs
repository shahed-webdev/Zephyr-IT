using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            this.MonthlySummary = new HashSet<MonthlyDashboardSummaryViewModel>();
            this.MonthlySales = new HashSet<MonthlySellingViewModel>();
            this.TopDueCustomer = new HashSet<CustomerDueViewModel>();
        }
        public double Sale { get; set; }
        public double Purchase { get; set; }
        public double Profit { get; set; }
        public double Expense { get; set; }
        public double MarketDue { get; set; }
        public double CustomerDue { get; set; }
        public double StockProductPurchaseValue { get; set; }
        public ICollection<MonthlyDashboardSummaryViewModel> MonthlySummary { get; set; }
        public ICollection<MonthlySellingViewModel> MonthlySales { get; set; }
        public ICollection<CustomerDueViewModel> TopDueCustomer { get; set; }
    }

    public class MonthlyDashboardSummaryViewModel
    {
        public MonthName Month { get; set; }
        public double MonthlySale { get; set; }
        public double MonthlySoldPurchasePrice { get; set; }
        public double MonthlyNewPurchase { get; set; }
        public double MonthlyExpense { get; set; }
        public double MonthlyProfit { get; set; }
        public double DailyAverageProfit { get; set; }
    }

    public class MonthlySellingViewModel
    {
        public MonthName Month { get; set; }
        public double MonthlySale { get; set; }
        public double MonthlySellingQuantity { get; set; }
    }
}
