﻿using System;
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
                TopDueCustomer = _db.Customers.TopDue(13),
                DueVendors = _db.Vendors.TopDue(6)
            };
            return dashboard;
        }

        public DailyDashboardSummaryViewModel DailyData()
        {
            _reportDay = DateTime.Now;
            var sale = _db.Selling.DailySaleAmount(_reportDay);
            var purchase = _db.Purchases.DailyPurchaseAmount(_reportDay);
            var profit = _db.Selling.DailyProfit(_reportDay);
            var expense = _db.Expenses.DailyExpenseAmount(_reportDay);
            var dailySummary = new DailyDashboardSummaryViewModel
            {
                Sale = Math.Round(sale, 2),
                Purchase = Math.Round(purchase, 2),
                Profit = Math.Round(profit, 2),
                Expense = Math.Round(expense, 2)
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

                          join pt in _db.Selling.MonthlyProfit(_reportYear) on m.MonthNumber equals pt.MonthNumber
                              into profit
                          from pt in profit.DefaultIfEmpty()

                          select new MonthlyDashboardSummaryViewModel
                          {
                              Month = m,
                              MonthlySale = Math.Round(s?.Amount ?? 0, 2),
                              MonthlyNewPurchase = Math.Round(p?.Amount ?? 0, 2),
                              MonthlyExpense = Math.Round(e?.Amount ?? 0, 2),
                              MonthlyProfit = Math.Round(pt?.Amount ?? 0, 2),
                              DailyAverageProfit = Math.Round(pt?.Amount ?? 0/30, 2)
                          }).ToList();

            return result ?? new List<MonthlyDashboardSummaryViewModel>(); ;
        }

    }
}