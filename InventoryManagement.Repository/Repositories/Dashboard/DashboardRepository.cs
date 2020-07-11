using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class DashboardRepository
    {
        private IUnitOfWork _db;
        private int _reportYear;
        private DateTime _reportDay;
        public ICollection<DDL> Years { get; }
        public DashboardRepository(IUnitOfWork db)
        {
            _db = db;
            Years = YearsDdls();
        }

        private ICollection<DDL> YearsDdls()
        {
            var years = _db.Expenses.Years();
            years = years.Union(_db.Selling.Years()).ToList();
            years = years.Union(_db.Purchases.Years()).ToList();
            years = years.Union(_db.Selling.Years()).ToList();

            return years.Select(y => new DDL
            {
                value = y,
                label = "Year: " + y.ToString()
            }).ToList();
        }

        public DashboardViewModel Data(int? year)
        {
            _reportYear = year ?? DateTime.Now.Year;


            var dashboard = new DashboardViewModel
            {
                DailySummary = DailyData(),
                MarketDue = _db.Vendors.TotalDue(),
                CustomerDue = _db.Customers.TotalDue(),
                StockProductPurchaseValue = _db.ProductStocks.StockProductPurchaseValue(),
                MonthlySummary = GetMonthlySummary(),
                TopDueCustomer = _db.Customers.TopDue(13)
            };
            return dashboard;
        }

        public DailyDashboardSummaryViewModel DailyData()
        {
            _reportDay = DateTime.Now;
            var sale = _db.Selling.DailySaleAmount(_reportDay);
            var purchase = _db.Selling.DailySoldPurchaseAmount(_reportDay);
            var profit = sale - purchase;
            var expense = _db.Expenses.DailyExpenseAmount(_reportDay);
            var dailySummary = new DailyDashboardSummaryViewModel
            {
                Sale = sale,
                Purchase = purchase,
                Profit = profit,
                Expense = expense
            };
            return dailySummary;

        }

        private ICollection<MonthlyDashboardSummaryViewModel> GetMonthlySummary()
        {
            var months = new AllMonth();

            var a = months.AllMonthNames;
            var b = _db.Expenses.MonthlyAmounts(_reportYear);
            var c = _db.Selling.MonthlyAmounts(_reportYear);

            var result = (from m in months.AllMonthNames

                          join e in _db.Expenses.MonthlyAmounts(_reportYear) on m.MonthNumber equals e.MonthNumber
                              into expanse
                          from e in expanse.DefaultIfEmpty()

                          join s in _db.Selling.MonthlyAmounts(_reportYear) on m.MonthNumber equals s.MonthNumber
                              into sell
                          from s in sell.DefaultIfEmpty()

                          join p in _db.Purchases.MonthlyAmounts(_reportYear) on m.MonthNumber equals p.MonthNumber
                              into purchase
                          from p in purchase.DefaultIfEmpty()

                          select new MonthlyDashboardSummaryViewModel
                          {
                              Month = m,
                              MonthlySale = s?.Amount ?? 0,
                              MonthlySoldPurchasePrice = 0,
                              MonthlyNewPurchase = p?.Amount ?? 0,
                              MonthlyExpense = e?.Amount ?? 0,
                              MonthlyProfit = 0,
                              DailyAverageProfit = 0
                          }).ToList();

            return result ?? new List<MonthlyDashboardSummaryViewModel>(); ;
        }

    }
}