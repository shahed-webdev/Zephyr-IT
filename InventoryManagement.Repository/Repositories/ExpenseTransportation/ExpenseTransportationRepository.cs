using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class ExpenseTransportationRepository : Repository<ExpenseTransportation>, IExpenseTransportationRepository
    {
        protected readonly IMapper _mapper;
        public ExpenseTransportationRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public void AddCustom(ExpenseTransportationAddModel model, int reregistrationId, int voucherNo, bool isApproved)
        {
            var expenseTransportation = _mapper.Map<ExpenseTransportation>(model);

            expenseTransportation.RegistrationId = reregistrationId;
            expenseTransportation.VoucherNo = voucherNo;
            expenseTransportation.IsApproved = isApproved;

            Context.ExpenseTransportation.Add(expenseTransportation);
        }

        public decimal Approved(int expenseTransportationId, int? accountId)
        {
            var expenseTransportation = Context.ExpenseTransportation.Find(expenseTransportationId);
            expenseTransportation.IsApproved = true;
            expenseTransportation.AccountId = accountId;
            Context.ExpenseTransportation.Update(expenseTransportation);

            return expenseTransportation.TotalExpense;
        }

        public void Delete(int expenseTransportationId)
        {
            var expenseTransportation = Context.ExpenseTransportation.Find(expenseTransportationId);
            Context.ExpenseTransportation.Remove(expenseTransportation);
        }

        public ExpenseTransportationDetailsModel GetDetails(int expenseTransportationId)
        {
            return Context.ExpenseTransportation
                .Include(e => e.ExpenseTransportationList)
                .Where(e => e.ExpenseTransportationId == expenseTransportationId)
                .ProjectTo<ExpenseTransportationDetailsModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public ExpenseUpdateAccountUpdateModel Edit(ExpenseTransportationDetailsModel model)
        {
            var expenseTransportation = Context.ExpenseTransportation
                .Include(e => e.ExpenseTransportationList)
                .FirstOrDefault(e => e.ExpenseTransportationId == model.ExpenseTransportationId);

            if (expenseTransportation == null) return null;

            var returnModel = new ExpenseUpdateAccountUpdateModel
            {
                IsApproved = expenseTransportation.IsApproved,
                PrevAmount = expenseTransportation.TotalExpense,
                CurrentAmount = model.TotalExpense,
                PrevAccountId = expenseTransportation.AccountId,
                CurrentAccountId = model.AccountId
            };

            expenseTransportation.ExpenseTransportationId = model.ExpenseTransportationId;
            expenseTransportation.CustomerId = model.CustomerId;
            expenseTransportation.TotalExpense = model.TotalExpense;
            expenseTransportation.ExpenseNote = model.ExpenseNote;
            expenseTransportation.ExpenseDate = model.ExpenseDate;
            expenseTransportation.ExpenseTransportationList = model.ExpenseTransportationList.Select(e => _mapper.Map<ExpenseTransportationList>(e)).ToList();

            Context.ExpenseTransportation.Update(expenseTransportation);
            return returnModel;
        }
    }
}