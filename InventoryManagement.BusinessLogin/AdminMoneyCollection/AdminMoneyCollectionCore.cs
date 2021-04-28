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
                return _db.AdminMoneyCollection.IsNull(id) ? new DbResponse(false, "No data Found") : _db.AdminMoneyCollection.Delete(id);
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