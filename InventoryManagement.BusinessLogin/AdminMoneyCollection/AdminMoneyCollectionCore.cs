using AutoMapper;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System;

namespace InventoryManagement.BusinessLogin
{
    public class AdminMoneyCollectionCore : CoreDependency, IAdminMoneyCollectionCore
    {
        public AdminMoneyCollectionCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
        {
        }

        public DbResponse<AdminMoneyCollectionViewModel> Add(AdminMoneyCollectionAddModel model)
        {
            try
            {
                if (model.CollectionAmount <= 0)
                    return new DbResponse<AdminMoneyCollectionViewModel>(false, "Invalid Data");


                _db.Registrations.BalanceSubtract(model.RegistrationId, model.CollectionAmount);

                return _db.AdminMoneyCollection.Add(model);

            }
            catch (Exception e)
            {
                return new DbResponse<AdminMoneyCollectionViewModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse Delete(int id)
        {
            try
            {
                if (_db.AdminMoneyCollection.IsNull(id))
                    return new DbResponse(false, "No data Found");
                var adminMoneyCollection = _db.AdminMoneyCollection.Get(id);
                _db.Registrations.BalanceAdd(adminMoneyCollection.RegistrationId, adminMoneyCollection.CollectionAmount);
                return _db.AdminMoneyCollection.Delete(id);
            }
            catch (Exception e)
            {
                return new DbResponse(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DataResult<AdminMoneyCollectionViewModel> List(DataRequest request)
        {
            return _db.AdminMoneyCollection.List(request);
        }
    }
}