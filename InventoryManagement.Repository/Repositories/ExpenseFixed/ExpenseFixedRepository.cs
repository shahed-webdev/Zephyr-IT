using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class ExpenseFixedRepository : Repository<ExpenseFixed>, IExpenseFixedRepository
    {
        protected readonly IMapper _mapper;
        public ExpenseFixedRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public ExpenseFixed ExpenseFixed { get; set; }

        public void Add(ExpenseFixedAddModel model)
        {
            ExpenseFixed = _mapper.Map<ExpenseFixed>(model);
            Context.ExpenseFixed.Add(ExpenseFixed);
        }

        public void Delete(int expenseFixedId)
        {
            var expense = Context.ExpenseFixed.Find(expenseFixedId);
            Context.ExpenseFixed.Remove(expense);
        }

        public ICollection<ExpenseFixedViewModel> Records()
        {
            return Context.ExpenseFixed
                .ProjectTo<ExpenseFixedViewModel>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}