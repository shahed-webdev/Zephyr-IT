using System;
using AutoMapper;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;

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
                    return new DbResponse(false, "Failed, Area already exist in this region");

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

        public DataResult<AccountCrudModel> List(DataRequest request)
        {
            return _db.Account.List(request);
        }
    }
}