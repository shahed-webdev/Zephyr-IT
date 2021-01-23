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
            var expense = Context.Expense.Include(e => e.ExpenseCategory).Select(e => new ExpenseViewModel
            {
                ExpenseId = e.ExpenseId,
                RegistrationId = e.RegistrationId,
                ExpenseCategoryId = e.ExpenseCategoryId,
                CategoryName = e.ExpenseCategory.CategoryName,
                ExpenseAmount = e.ExpenseAmount,
                ExpenseFor = e.ExpenseFor,
                ExpenseDate = e.ExpenseDate
            }).ToList();

            return expense;
        }

        public DataResult<ExpenseAllViewModel> RecordsDataTable(DataRequest request)
        {
            return Context.ExpenseTransportation
                .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
                .Union(Context.Expense
                    .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider))
                .OrderByDescending(r => r.ExpenseDate)
                .ToDataResult(request);

        }

        public List<ExpenseAllViewModel> Records()
        {
            var records = new List<ExpenseAllViewModel>();
            var t = Context.ExpenseTransportation
                  .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
                  .ToList();

            var g = Context.Expense
                .ProjectTo<ExpenseAllViewModel>(_mapper.ConfigurationProvider)
                .ToList();
            records.AddRange(g);
            records.AddRange(t);

            return records.OrderByDescending(r => r.ExpenseDate).ToList();
        }

        public Task<List<ExpenseViewModel>> ToListCustomAsync()
        {
            var expense = Context.Expense.Include(e => e.ExpenseCategory).Select(e => new ExpenseViewModel
            {
                ExpenseId = e.ExpenseId,
                RegistrationId = e.RegistrationId,
                ExpenseCategoryId = e.ExpenseCategoryId,
                CategoryName = e.ExpenseCategory.CategoryName,
                ExpenseAmount = e.ExpenseAmount,
                ExpenseFor = e.ExpenseFor,
                ExpenseDate = e.ExpenseDate
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
                IsApproved = isApproved
            });
        }

        public void Approved(int expenseId)
        {
            var expense = Find(expenseId);
            expense.IsApproved = true;
            Update(expense);
        }

        public void Edit(ExpenseAddModel model)
        {
            var expense = Find(model.ExpenseId);
            expense.ExpenseDate = model.ExpenseDate;
            expense.ExpenseAmount = model.ExpenseAmount;
            expense.ExpenseCategoryId = model.ExpenseCategoryId;
            expense.ExpenseFor = model.ExpenseFor;
            Update(expense);
        }

        public ExpenseAddModel GetDetails(int expenseId)
        {
            var expense = Find(expenseId);

            return new ExpenseAddModel
            {
                ExpenseId = expense.ExpenseId,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                CategoryName = expense.ExpenseCategory.CategoryName,
                ExpenseAmount = expense.ExpenseAmount,
                ExpenseFor = expense.ExpenseFor,
                ExpenseDate = expense.ExpenseDate
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

        public double DailyExpenseAmount(DateTime? date)
        {
            var saleDate = date ?? DateTime.Now;
            return Context.Expense.Where(e => e.ExpenseDate.Date == saleDate.Date)?
                       .Sum(e => e.ExpenseAmount) ?? 0;
        }

        public double ExpenseYearly(int year)
        {
            return ToList().Where(s => s.ExpenseDate.Year == year).Sum(s => s.ExpenseAmount);
        }

        public ICollection<MonthlyAmount> MonthlyAmounts(int year)
        {
            var months = Context.Expense.Where(e => e.ExpenseDate.Year == year)
                .GroupBy(e => new
                {
                    number = e.ExpenseDate.Month

                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = g.Sum(e => e.ExpenseAmount)
                })
                .ToList();

            return months;
        }

        public ICollection<ExpenseViewModel> DateToDate(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);

            var expense = Context.Expense.Include(e => e.ExpenseCategory).Where(e => e.ExpenseDate <= eD && e.ExpenseDate >= sD).Select(e => new ExpenseViewModel
            {
                ExpenseId = e.ExpenseId,
                RegistrationId = e.RegistrationId,
                ExpenseCategoryId = e.ExpenseCategoryId,
                CategoryName = e.ExpenseCategory.CategoryName,
                ExpenseAmount = e.ExpenseAmount,
                ExpenseFor = e.ExpenseFor,
                ExpenseDate = e.ExpenseDate
            }).ToList();

            return expense;
        }
        public ICollection<ExpenseCategoryWise> CategoryWistDateToDate(DateTime? sDateTime, DateTime? eDateTime)
        {
            var sD = sDateTime ?? new DateTime(1000, 1, 1);
            var eD = eDateTime ?? new DateTime(3000, 1, 1);

            var ex = Context.Expense.Include(e => e.ExpenseCategory).Where(e => e.ExpenseDate <= eD && e.ExpenseDate >= sD)
                .GroupBy(e => new
                {
                    ExpenseCategoryID = e.ExpenseCategoryId,
                    e.ExpenseCategory.CategoryName

                })
                .Select(g => new ExpenseCategoryWise
                {
                    ExpenseCategoryId = g.Key.ExpenseCategoryID,
                    CategoryName = g.Key.CategoryName,
                    TotalExpense = g.Sum(e => e.ExpenseAmount)

                })
                .ToList();

            return ex;
        }
    }
}
