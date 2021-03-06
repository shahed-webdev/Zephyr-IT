using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class AccountWithdrawRepository : Repository<AccountWithdraw>, IAccountWithdrawRepository
    {

        protected readonly IMapper _mapper;

        public AccountWithdrawRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public DbResponse<AccountWithdrawCrudModel> Add(AccountWithdrawCrudModel model)
        {
            var accountWithdraw = _mapper.Map<AccountWithdraw>(model);
            Context.AccountWithdraw.Add(accountWithdraw);
            Context.SaveChanges();
            model.AccountWithdrawId = accountWithdraw.AccountWithdrawId;

            return new DbResponse<AccountWithdrawCrudModel>(true, $"{model.WithdrawAmount} Amount Withdrawn Successfully", model);
        }

        public DbResponse Delete(int id)
        {
            var accountWithdraw = Context.AccountWithdraw.Find(id);
            Context.AccountWithdraw.Remove(accountWithdraw);
            Context.SaveChanges();
            return new DbResponse(true, $"{accountWithdraw.WithdrawAmount} Amount Deleted Successfully");
        }

        public bool IsNull(int id)
        {
            return !Context.AccountWithdraw.Any(r => r.AccountWithdrawId == id);
        }

        public DataResult<AccountWithdrawCrudModel> List(DataRequest request)
        {
            return Context.AccountWithdraw
                .ProjectTo<AccountWithdrawCrudModel>(_mapper.ConfigurationProvider)
                .OrderBy(a => a.WithdrawDateUtc)
                .ToDataResult(request);
        }

        public DbResponse<AccountWithdrawCrudModel> Get(int id)
        {
            var accountDeposit = Context.AccountWithdraw.Where(r => r.AccountWithdrawId == id)
                .ProjectTo<AccountWithdrawCrudModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
            return new DbResponse<AccountWithdrawCrudModel>(true, $"{accountDeposit.WithdrawAmount} Get Successfully", accountDeposit);
        }
    }
}