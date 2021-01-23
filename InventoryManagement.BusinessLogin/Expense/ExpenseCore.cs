using AutoMapper;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
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
            try
            {
                _db.ExpenseTransportations.Approved(expenseTransportationId);
                _db.SaveChanges();

                return new DbResponse(true, "Approved Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse DeleteTransportationCost(int expenseTransportationId)
        {
            try
            {
                _db.ExpenseTransportations.Delete(expenseTransportationId);
                _db.SaveChanges();

                return new DbResponse(true, "Deleted Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse<ExpenseTransportationDetailsModel> GetTransportationCostDetails(int expenseTransportationId)
        {
            try
            {
                var data = _db.ExpenseTransportations.GetDetails(expenseTransportationId);
                return new DbResponse<ExpenseTransportationDetailsModel>(true, "Success", data);
            }
            catch (Exception e)
            {
                return new DbResponse<ExpenseTransportationDetailsModel>(false, e.Message);
            }
        }

        public DbResponse EditTransportationCost(ExpenseTransportationDetailsModel model)
        {
            try
            {
                _db.ExpenseTransportations.Edit(model);
                return new DbResponse(true, "Changed Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse AddFixedCost(ExpenseFixedAddModel model)
        {
            try
            {
                _db.ExpenseFixed.Add(model);
                _db.SaveChanges();

                return new DbResponse(true, "Added Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse DeleteFixedCost(int expenseFixedId)
        {
            try
            {
                _db.ExpenseFixed.Delete(expenseFixedId);
                _db.SaveChanges();

                return new DbResponse(true, "Deleted Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse<DataResult<ExpenseFixedViewModel>> FixedCostRecords(DataRequest request)
        {
            try
            {
                var data = _db.ExpenseFixed.RecordDataTable(request);
                return new DbResponse<DataResult<ExpenseFixedViewModel>>(true, "Success", data);
            }
            catch (Exception e)
            {
                return new DbResponse<DataResult<ExpenseFixedViewModel>>(false, e.Message);
            }
        }
    }
}