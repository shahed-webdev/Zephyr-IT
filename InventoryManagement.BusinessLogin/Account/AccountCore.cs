using AutoMapper;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;

namespace InventoryManagement.BusinessLogin
{
    public class AccountCore : CoreDependency, IAccountCore
    {
        public AccountCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
        {
        }

        public DbResponse<AccountCrudModel> Add(AccountCrudModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AccountName))
                    return new DbResponse<AccountCrudModel>(false, "Invalid Data");

                if (_db.Account.IsExistName(model.AccountName))
                    return new DbResponse<AccountCrudModel>(false, $" {model.AccountName} already Exist");

                return _db.Account.Add(model);

            }
            catch (Exception e)
            {
                return new DbResponse<AccountCrudModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse Edit(AccountCrudModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AccountName))
                    return new DbResponse(false, "Invalid Data");

                if (_db.Account.IsNull(model.AccountId))
                    return new DbResponse(false, "No data Found");

                if (_db.Account.IsExistName(model.AccountName, model.AccountId))
                    return new DbResponse(false, $" {model.AccountName} already Exist");


                return _db.Account.Edit(model);

            }
            catch (Exception e)
            {
                return new DbResponse(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse Delete(int id)
        {
            try
            {
                if (_db.Account.IsNull(id))
                    return new DbResponse(false, "No data Found");

                if (_db.Account.IsRelatedDataExist(id))
                    return new DbResponse(false, "Failed, Related Data exist");

                return _db.Account.Delete(id);

            }
            catch (Exception e)
            {
                return new DbResponse(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse<AccountCrudModel> Get(int id)
        {
            try
            {
                if (_db.Account.IsNull(id))
                    return new DbResponse<AccountCrudModel>(false, "No data Found");

                return _db.Account.Get(id);

            }
            catch (Exception e)
            {
                return new DbResponse<AccountCrudModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public List<AccountCrudModel> List()
        {
            return _db.Account.List();
        }

        public List<DDL> DdlList()
        {
            return _db.Account.DdlList();
        }

        public DbResponse<AccountDepositCrudModel> Deposit(AccountDepositCrudModel model)
        {
            try
            {
                if (model.DepositAmount < 0)
                    return new DbResponse<AccountDepositCrudModel>(false, "Invalid Data");

                if (_db.Account.IsNull(model.AccountId))
                    return new DbResponse<AccountDepositCrudModel>(false, $"Account Not Found");

                _db.Account.BalanceAdd(model.AccountId, model.DepositAmount);

                return _db.AccountDeposit.Add(model);

            }
            catch (Exception e)
            {
                return new DbResponse<AccountDepositCrudModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse<AccountWithdrawCrudModel> Withdraw(AccountWithdrawCrudModel model)
        {
            try
            {
                if (model.WithdrawAmount < 0)
                    return new DbResponse<AccountWithdrawCrudModel>(false, "Invalid Data");

                if (_db.Account.IsNull(model.AccountId))
                    return new DbResponse<AccountWithdrawCrudModel>(false, $"Account Not Found");

                _db.Account.BalanceSubtract(model.AccountId, model.WithdrawAmount);

                return _db.AccountWithdraw.Add(model);

            }
            catch (Exception e)
            {
                return new DbResponse<AccountWithdrawCrudModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse DepositDelete(int id)
        {
            try
            {
                var accountDeposit = _db.AccountDeposit.Get(id);

                if (!accountDeposit.IsSuccess) new DbResponse(false, accountDeposit.Message);

                _db.Account.BalanceSubtract(accountDeposit.Data.AccountId, accountDeposit.Data.DepositAmount);

                return _db.AccountDeposit.Delete(id);

            }
            catch (Exception e)
            {
                return new DbResponse(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse WithdrawDelete(int id)
        {
            try
            {
                var accountWithdraw = _db.AccountWithdraw.Get(id);

                if (!accountWithdraw.IsSuccess) new DbResponse(false, accountWithdraw.Message);

                _db.Account.BalanceAdd(accountWithdraw.Data.AccountId, accountWithdraw.Data.WithdrawAmount);

                return _db.AccountWithdraw.Delete(id);

            }
            catch (Exception e)
            {
                return new DbResponse(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DataResult<AccountDepositCrudModel> DepositList(DataRequest request)
        {
            return _db.AccountDeposit.List(request);
        }

        public DataResult<AccountWithdrawCrudModel> WithdrawList(DataRequest request)
        {
            return _db.AccountWithdraw.List(request);
        }

        public void DefaultAccountSet(int accountId)
        {
            _db.Account.DefaultAccountSet(accountId);
        }

        public int DefaultAccountGet()
        {
            return _db.Account.DefaultAccountGet();
        }

        public void CapitalSet(decimal amount)
        {
            _db.Account.CapitalSet(amount);
        }

        public decimal CapitalGet()
        {
            return _db.Account.CapitalGet();
        }

        public DbResponse TransferToDefault(int accountId, decimal amount)
        {
            return _db.Account.TransferToDefault(accountId, amount);
        }
    }
}