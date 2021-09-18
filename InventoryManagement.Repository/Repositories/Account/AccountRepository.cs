using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        protected readonly IMapper _mapper;
        public AccountRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public DbResponse<AccountCrudModel> Add(AccountCrudModel model)
        {
            var account = _mapper.Map<Account>(model);
            Context.Account.Add(account);
            Context.SaveChanges();
            model.AccountId = account.AccountId;

            return new DbResponse<AccountCrudModel>(true, $"{model.AccountName} Added Successfully", model);
        }

        public DbResponse Edit(AccountCrudModel model)
        {
            var account = Context.Account.Find(model.AccountId);
            account.AccountName = model.AccountName;
            account.CostPercentage = model.CostPercentage;
            Context.Account.Update(account);
            Context.SaveChanges();
            return new DbResponse(true, $"{account.AccountName} Updated Successfully");
        }

        public DbResponse Delete(int id)
        {
            var account = Context.Account.Find(id);
            Context.Account.Remove(account);
            Context.SaveChanges();
            return new DbResponse(true, $"{account.AccountName} Deleted Successfully");
        }

        public DbResponse<AccountCrudModel> Get(int id)
        {
            var account = Context.Account.Where(r => r.AccountId == id)
                .ProjectTo<AccountCrudModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
            return new DbResponse<AccountCrudModel>(true, $"{account.AccountName} Get Successfully", account);
        }

        public bool IsExistName(string name)
        {
            return Context.Account.Any(r => r.AccountName == name);
        }

        public bool IsExistName(string name, int updateId)
        {
            return Context.Account.Any(r => r.AccountName == name && r.AccountId != updateId);
        }

        public bool IsNull(int id)
        {
            return !Context.Account.Any(r => r.AccountId == id);
        }

        public bool IsRelatedDataExist(int id)
        {
            return Context.AccountDeposit.Any(a => a.AccountId == id) ||
                   Context.AccountWithdraw.Any(a => a.AccountId == id) ||
                   Context.SellingPayment.Any(a => a.AccountId == id) ||
                   Context.SellingExpense.Any(a => a.AccountId == id);

        }

        public List<AccountCrudModel> List()
        {
            return Context.Account
                .ProjectTo<AccountCrudModel>(_mapper.ConfigurationProvider)
                .OrderBy(a => a.AccountName)
                .ToList();
        }

        public List<DDL> DdlList()
        {
            return Context.Account.Select(a => new DDL
            {
                value = a.AccountId,
                label = a.AccountName
            }).ToList();
        }

        public void BalanceAdd(int id, decimal amount)
        {
            var account = Context.Account.Find(id);
            account.Balance += amount;
            Context.Account.Update(account);
        }

        public void BalanceSubtract(int id, decimal amount)
        {
            var account = Context.Account.Find(id);
            account.Balance -= amount;
            Context.Account.Update(account);
        }

        public decimal GetCostPercentage(int id)
        {
            var account = Context.Account.Find(id);
            return account?.CostPercentage ?? 0;
        }

        public void DefaultAccountSet(int accountId)
        {
            var institution = Context.Institution.FirstOrDefault();
            if (!Context.Account.Any(a => a.AccountId == accountId)) return;
            institution.DefaultAccountId = accountId;
            Context.Institution.Update(institution);
            Context.SaveChanges();

        }

        public int DefaultAccountGet()
        {
            return Context.Institution.FirstOrDefault()?.DefaultAccountId ?? 0;
        }

        public void CapitalSet(decimal amount)
        {
            var institution = Context.Institution.FirstOrDefault();
            institution.Capital = amount;
            Context.Institution.Update(institution);
            Context.SaveChanges();
        }

        public decimal CapitalGet()
        {
            return Context.Institution.FirstOrDefault()?.Capital ?? 0;
        }

        public void SellingReturnRecordAdd(SellingPaymentReturnRecordModel model)
        {
            var net = model.CurrentReturnAmount - model.PrevReturnAmount;
            if (net == 0) return;

            var returnRecord = _mapper.Map<SellingPaymentReturnRecord>(model);
            Context.SellingPaymentReturnRecord.Add(returnRecord);

            if (model.AccountId != null) BalanceSubtract(model.AccountId.Value, net);
            Context.SaveChanges();
        }

        public void PurchaseReturnRecordAdd(PurchasePaymentReturnRecordModel model)
        {
            var net = model.CurrentReturnAmount - model.PrevReturnAmount;
            if (net == 0) return;

            var returnRecord = _mapper.Map<PurchasePaymentReturnRecord>(model);
            Context.PurchasePaymentReturnRecord.Add(returnRecord);

            if (model.AccountId != null) BalanceAdd(model.AccountId.Value, net);

            Context.SaveChanges();
        }

        public DbResponse TransferToDefault(int accountId, decimal amount)
        {
            var defaultAccountId = this.DefaultAccountGet();

            if (defaultAccountId == 0) return new DbResponse(false, $"Default Account not found");

            var fromAccount = Context.Account.Find(accountId);

            if (fromAccount == null) return new DbResponse(false, $"Account information not valid");

            if (fromAccount.Balance < amount) return new DbResponse(false, $"Not sufficient balance");

            this.BalanceAdd(defaultAccountId, amount);

            this.BalanceSubtract(fromAccount.AccountId, amount);

            Context.SaveChanges();

            return new DbResponse(true, $"{amount} Tk. Transfer Successfully");
        }
    }
}