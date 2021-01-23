using AutoMapper;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.BusinessLogin
{
    public class ExpenseCore : CoreDependency, IExpenseCore
    {
        public ExpenseCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
        {
        }

        public DbResponse<DataResult<ExpenseAllViewModel>> ExpenseRecords(DataRequest request)
        {
            try
            {
                var data = _db.Expenses.RecordsDataTable(request);
                return new DbResponse<DataResult<ExpenseAllViewModel>>(true, "Success", data);
            }
            catch (Exception e)
            {
                return new DbResponse<DataResult<ExpenseAllViewModel>>(false, e.Message);
            }
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

        public DbResponse<ExpenseFixedViewModel> AddFixedCost(ExpenseFixedAddModel model)
        {
            try
            {
                _db.ExpenseFixed.Add(model);
                _db.SaveChanges();
                var data = _mapper.Map<ExpenseFixedViewModel>(_db.ExpenseFixed.ExpenseFixed);
                return new DbResponse<ExpenseFixedViewModel>(true, "Added Successfully", data);
            }
            catch (Exception e)
            {
                return new DbResponse<ExpenseFixedViewModel>(false, e.Message);
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

        public DbResponse<List<ExpenseFixedViewModel>> FixedCostRecords()
        {
            try
            {
                var data = _db.ExpenseFixed.Records();
                return new DbResponse<List<ExpenseFixedViewModel>>(true, "Success", data.ToList());
            }
            catch (Exception e)
            {
                return new DbResponse<List<ExpenseFixedViewModel>>(false, e.Message);
            }
        }
    }
}