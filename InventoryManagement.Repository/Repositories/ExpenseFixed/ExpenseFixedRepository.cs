using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;

namespace InventoryManagement.Repository
{
    public class ExpenseFixedRepository : Repository<ExpenseFixed>, IExpenseFixedRepository
    {
        protected readonly IMapper _mapper;
        public ExpenseFixedRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public void Add(ExpenseFixedAddModel model)
        {
            var expenseFixed = _mapper.Map<ExpenseFixed>(model);
            Context.ExpenseFixed.Add(expenseFixed);
        }

        public void Delete(int expenseFixedId)
        {
            var expense = Context.ExpenseFixed.Find(expenseFixedId);
            Context.ExpenseFixed.Remove(expense);
        }

        public DataResult<ExpenseFixedViewModel> RecordDataTable(DataRequest request)
        {
            return Context.ExpenseFixed
                .ProjectTo<ExpenseFixedViewModel>(_mapper.ConfigurationProvider)
                .ToDataResult(request);
        }
    }
}