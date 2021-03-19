using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class AccountDepositRepository : Repository<AccountDeposit>, IAccountDepositRepository
    {

        protected readonly IMapper _mapper;

        public AccountDepositRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public DbResponse<AccountDepositCrudModel> Add(AccountDepositCrudModel model)
        {
            var accountDeposit = _mapper.Map<AccountDeposit>(model);
            Context.AccountDeposit.Add(accountDeposit);
            Context.SaveChanges();
            model.AccountDepositId = accountDeposit.AccountDepositId;

            return new DbResponse<AccountDepositCrudModel>(true, $"{model.DepositAmount} Amount Deposited Successfully", model);
        }

        public DbResponse Delete(int id)
        {
            var accountDeposit = Context.AccountDeposit.Find(id);
            Context.AccountDeposit.Remove(accountDeposit);
            Context.SaveChanges();
            return new DbResponse(true, $"{accountDeposit.DepositAmount} Amount Deleted Successfully");
        }

        public bool IsNull(int id)
        {
            return !Context.AccountDeposit.Any(r => r.AccountDepositId == id);
        }

        public DataResult<AccountDepositCrudModel> List(DataRequest request)
        {
            return Context.AccountDeposit
                .ProjectTo<AccountDepositCrudModel>(_mapper.ConfigurationProvider)
                .OrderBy(a => a.DepositDateUtc)
                .ToDataResult(request);
        }

        public DbResponse<AccountDepositCrudModel> Get(int id)
        {
            var accountDeposit = Context.AccountDeposit.Where(r => r.AccountDepositId == id)
                .ProjectTo<AccountDepositCrudModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
            return new DbResponse<AccountDepositCrudModel>(true, $"{accountDeposit.DepositAmount} Get Successfully", accountDeposit);
        }
    }
}