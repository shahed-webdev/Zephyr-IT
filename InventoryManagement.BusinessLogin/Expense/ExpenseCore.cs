using AutoMapper;
using InventoryManagement.Repository;
using System;

namespace InventoryManagement.BusinessLogin
{
    public class ExpenseCore : CoreDependency, IExpenseCore
    {
        public ExpenseCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
        {
        }

        public DbResponse AddTransportationCost(ExpenseTransportationAddModel model, string userName, bool isApproved)
        {
            try
            {
                var registrationId = _db.Registrations.GetRegID_ByUserName(userName);
                var voucherNo = _db.Institutions.GetVoucherCountdown() + 1;

                _db.ExpenseTransportations.AddCustom(model, registrationId, voucherNo, isApproved);
                _db.SaveChanges();

                return new DbResponse(true, "Added Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }

        }

        public DbResponse ApprovedTransportationCost(int expenseTransportationId)
        {
            throw new System.NotImplementedException();
        }

        public DbResponse DeleteTransportationCost(int expenseTransportationId)
        {
            throw new System.NotImplementedException();
        }

        public DbResponse<ExpenseTransportationDetailsModel> GetTransportationCostDetails(int expenseTransportationId)
        {
            throw new System.NotImplementedException();
        }

        public DbResponse EditTransportationCost(ExpenseTransportationDetailsModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}