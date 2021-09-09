using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        protected readonly IMapper _mapper;
        public ExpenseRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public ICollection<ExpenseViewModel> ToListCustom()
        {
            var expense = Context.Expense
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Account)
                .Select(e => new ExpenseViewModel
                {
                    ExpenseId = e.ExpenseId,
                    RegistrationId = e.RegistrationId,
                    ExpenseCategoryId = e.ExpenseCategoryId,
                    CategoryName = e.ExpenseCategory.CategoryName,
                    ExpenseAmount = e.ExpenseAmount,
                    ExpenseFor = e.ExpenseFor,
                    ExpenseDate = e.ExpenseDate,
                    AccountId = e.AccountId,
                    AccountName = e.Account.AccountName
                }).ToList();

            return expense;
        }

        public DataResult<ExpenseAllViewModel> RecordsDataTable(DataRequest request)
        {
            return Context.VW_ExpenseWithTransportation
                .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
                //.Union(Context.Expense
                //    .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider))
                //.OrderByDescending(r => r.ExpenseDate)
                .ToDataResult(request);

        }

        public List<ExpenseAllViewModel> Records()
        {
            var records = new List<ExpenseAllViewModel>();
            //var t = Context.ExpenseTransportation
            //      .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
            //      .ToList();

            //var g = Context.Expense
            //    .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
            //    .ToList();


            //records.AddRange(g);
            //records.AddRange(t);
            records = Context.VW_ExpenseWithTransportation
                .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider).ToList();

            return records.OrderByDescending(r => r.ExpenseDate).ToList();
        }

        public Task<List<ExpenseViewModel>> ToListCustomAsync()
        {
            var expense = Context.Expense
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Account)
                .Select(e => new ExpenseViewModel
                {
                    ExpenseId = e.ExpenseId,
                    RegistrationId = e.RegistrationId,
                    ExpenseCategoryId = e.ExpenseCategoryId,
                    CategoryName = e.ExpenseCategory.CategoryName,
                    ExpenseAmount = e.ExpenseAmount,
                    ExpenseFor = e.ExpenseFor,
                    ExpenseDate = e.ExpenseDate,
                    AccountId = e.AccountId,
                    AccountName = e.Account.AccountName
                }).ToListAsync();

            return expense;
        }

        public void AddCustom(ExpenseAddModel model, int registrationId, int voucherNo, bool isApproved)
        {
            Add(new Expense
            {
                RegistrationId = registrationId,
                ExpenseCategoryId = model.ExpenseCategoryId,
                ExpenseAmount = model.ExpenseAmount,
                ExpenseFor = model.ExpenseFor,
                ExpenseDate = model.ExpenseDate,
                VoucherNo = voucherNo,
                IsApproved = isApproved,
                AccountId = model.AccountId
            });
        }

        public decimal Approved(int expenseId, int? accountId)
        {
            var expense = Find(expenseId);
            expense.IsApproved = true;
            expense.AccountId = accountId;
            Update(expense);

            return expense.ExpenseAmount;
        }

        public ExpenseUpdateAccountUpdateModel Edit(ExpenseAddModel model)
        {
            var expense = Find(model.ExpenseId);
            if (expense == null) return null;

            var returnModel = new ExpenseUpdateAccountUpdateModel
            {
                IsApproved = expense.IsApproved,
                PrevAmount = expense.ExpenseAmount,
                CurrentAmount = model.ExpenseAmount,
                PrevAccountId = expense.AccountId,
                CurrentAccountId = model.AccountId
            };

            expense.ExpenseDate = model.ExpenseDate;
            expense.ExpenseAmount = model.ExpenseAmount;
            expense.ExpenseCategoryId = model.ExpenseCategoryId;
            expense.ExpenseFor = model.ExpenseFor;
            Update(expense);

            return returnModel;
        }

        public ExpenseDetailsModel GetDetails(int expenseId)
        {
            var expense = Context.Expense
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Registration)
                .Include(e => e.Account)
                .FirstOrDefault(e => e.ExpenseId == expenseId);

            return new ExpenseDetailsModel
            {
                ExpenseId = expense.ExpenseId,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                CategoryName = expense.ExpenseCategory.CategoryName,
                ExpenseAmount = expense.ExpenseAmount,
                ExpenseFor = expense.ExpenseFor,
                ExpenseDate = expense.ExpenseDate,
                IsApproved = expense.IsApproved,
                CreateBy = expense.Registration.UserName,
                AccountName = expense.Account?.AccountName,
                AccountId = expense.AccountId
            };
        }

        public void RemoveCustom(int id)
        {
            var expense = Find(id);
            Remove(expense);
        }

        public ICollection<int> Years()
        {
            var years = Context.Expense
                .GroupBy(e => new
                {
                    e.ExpenseDate.Year
                })
                .Select(g => g.Key.Year)
                .OrderBy(o => o)
                .ToList();

            var currentYear = DateTime.Now.Year;

            if (!years.Contains(currentYear)) years.Add(currentYear);

            return years;
        }

        public decimal DailyExpenseAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now;
            decimal ex = 0;


            ex += Context.Expense
                  .Where(e => e.IsApproved && e.ExpenseDate.Date == saleDate.Date)
                .Sum(e => e.ExpenseAmount);

            ex += Context.ExpenseTransportation
                .Where(e => e.IsApproved && e.ExpenseDate.Date == saleDate.Date)
                .Sum(e => e.TotalExpense);

            return ex;
        }

        public decimal FixedExpensePerDay()
        {
            return Context.ExpenseFixed?.Sum(f => f.CostPerDay) ?? 0;
        }

        public decimal ExpenseYearly(int year)
        {
            decimal ex = 0;
            ex += Context.Expense
                .Where(s => s.IsApproved && s.ExpenseDate.Year == year)
                .Sum(s => s.ExpenseAmount);

            ex += Context.ExpenseTransportation
                .Where(s => s.IsApproved && s.ExpenseDate.Year == year)
                .Sum(s => s.TotalExpense);
            return ex;
        }

        public ICollection<MonthlyAmount> MonthlyAmounts(int year)
        {

            var ex = new List<MonthlyAmount>();

            var exTransportation = Context.ExpenseTransportation
                .Where(e => e.IsApproved && e.ExpenseDate.Year == year)
                .Select(t => new MonthlyAmount
                {
                    MonthNumber = t.ExpenseDate.Month,
                    Amount = t.TotalExpense
                }).ToList();


            var exGeneral = Context.Expense
                .Where(e => e.IsApproved && e.ExpenseDate.Year == year)
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.ExpenseDate.Month,
                    Amount = g.ExpenseAmount
                }).ToList();


            ex.AddRange(exGeneral);
            ex.AddRange(exTransportation);

            var months = ex
                .GroupBy(e => new
                {
                    number = e.MonthNumber

                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = g.Sum(e => e.Amount)
                })
                .ToList();

            return months;
        }

        public ICollection<ExpenseViewModel> DateToDate(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);

            var expense = Context.Expense
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Account)
                .Where(e => e.ExpenseDate <= eD && e.ExpenseDate >= sD).Select(e => new ExpenseViewModel
                {
                    ExpenseId = e.ExpenseId,
                    RegistrationId = e.RegistrationId,
                    ExpenseCategoryId = e.ExpenseCategoryId,
                    CategoryName = e.ExpenseCategory.CategoryName,
                    ExpenseAmount = e.ExpenseAmount,
                    ExpenseFor = e.ExpenseFor,
                    ExpenseDate = e.ExpenseDate,
                    AccountName = e.Account.AccountName,
                    AccountId = e.AccountId
                }).ToList();

            return expense;
        }
        public ICollection<ExpenseCategoryWise> CategoryWistSummaryDateToDate(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);

            var ex = new List<ExpenseCategoryWise>();

            var exTransportation = Context.ExpenseTransportation
                .Where(e => e.IsApproved && e.ExpenseDate <= eD && e.ExpenseDate >= sD)
                .Select(t => new ExpenseCategoryWise
                {
                    //ExpenseCategoryId = 0,
                    CategoryName = "Transportation",
                    TotalExpense = t.TotalExpense
                }).ToList();


            var exGeneral = Context.Expense.Include(e => e.ExpenseCategory)
                .Where(e => e.IsApproved && e.ExpenseDate <= eD && e.ExpenseDate >= sD)
                .Select(g => new ExpenseCategoryWise
                {
                    //ExpenseCategoryId = g.ExpenseCategoryId,
                    CategoryName = g.ExpenseCategory.CategoryName,
                    TotalExpense = g.ExpenseAmount
                }).ToList();


            ex.AddRange(exGeneral);
            ex.AddRange(exTransportation);

            var report = ex.GroupBy(e => new
            {
                e.CategoryName

            }).Select(g => new ExpenseCategoryWise
            {
                CategoryName = g.Key.CategoryName,
                TotalExpense = g.Sum(e => e.TotalExpense)

            }).ToList();

            return report;
        }

        public ICollection<ExpenseAllViewModel> CategoryWistDetailsDateToDate(string category, DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);
            var records = new List<ExpenseAllViewModel>();
            var t = Context.ExpenseTransportation
                .Where(e => e.IsApproved && e.ExpenseDate <= eD && e.ExpenseDate >= sD)
                .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            var g = Context.Expense
                .Where(e => e.IsApproved && e.ExpenseDate <= eD && e.ExpenseDate >= sD)
                .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
                .ToList();


            records.AddRange(g);
            records.AddRange(t);

            return records.Where(r => r.ExpenseCategory == category).OrderByDescending(r => r.ExpenseDate).ToList();
        }
    }
}
