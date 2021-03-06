﻿using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class AccountRepository: Repository<Account>,IAccountRepository
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
                   Context.SellingPayment.Any(a => a.AccountId == id);

        }

        public List<AccountCrudModel> List()
        {
            return Context.Account              
                .ProjectTo<AccountCrudModel>(_mapper.ConfigurationProvider)
                .OrderBy(a => a.AccountName)
                .ToList();
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
    }
}