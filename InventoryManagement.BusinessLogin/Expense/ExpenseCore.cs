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

        public DbResponse<List<ExpenseAllViewModel>> ExpenseRecords()
        {
            try
            {
                var data = _db.Expenses.Records();
                return new DbResponse<List<ExpenseAllViewModel>>(true, "Success", data);
            }
            catch (Exception e)
            {
                return new DbResponse<List<ExpenseAllViewModel>>(false, e.Message);
            }
        }

        public DbResponse AddCost(ExpenseAddModel model, string userName, bool isApproved)
        {
            try
            {
                var registrationId = _db.Registrations.GetRegID_ByUserName(userName);
                var voucherNo = _db.Institutions.GetVoucherCountdown() + 1;

                _db.Expenses.AddCustom(model, registrationId, voucherNo, isApproved);

                if (isApproved && model.AccountId != null) _db.Account.BalanceSubtract(model.AccountId.Value, model.ExpenseAmount);

                _db.Institutions.IncreaseVoucherCount();
                _db.SaveChanges();

                return new DbResponse(true, "Added Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse ApprovedCost(int expenseId, int? accountId)
        {
            try
            {
                var amount = _db.Expenses.Approved(expenseId, accountId);

                if (accountId != null) _db.Account.BalanceSubtract(accountId.Value, amount);

                _db.SaveChanges();

                return new DbResponse(true, "Approved Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse DeleteCost(int expenseId)
        {
            try
            {
                _db.Expenses.RemoveCustom(expenseId);
                _db.SaveChanges();

                return new DbResponse(true, "Deleted Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse EditCost(ExpenseAddModel model)
        {
            try
            {
                var updateData = _db.Expenses.Edit(model);

                if (updateData == null) return new DbResponse(false, "No data found");

                if (updateData.IsApproved)
                {
                    if (updateData.PrevAccountId != null)
                    {
                        _db.Account.BalanceAdd(updateData.PrevAccountId.Value, updateData.PrevAmount);
                    }

                    _db.SaveChanges();

                    if (updateData.CurrentAccountId != null)
                    {
                        _db.Account.BalanceSubtract(updateData.CurrentAccountId.Value, updateData.CurrentAmount);
                    }
                }

                _db.SaveChanges();

                return new DbResponse(true, "Changed Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }
        }

        public DbResponse<ExpenseDetailsModel> GetCost(int expenseId)
        {
            try
            {
                var data = _db.Expenses.GetDetails(expenseId);


                return new DbResponse<ExpenseDetailsModel>(true, "Success", data);
            }
            catch (Exception e)
            {
                return new DbResponse<ExpenseDetailsModel>(false, e.Message);
            }
        }

        public DbResponse AddTransportationCost(ExpenseTransportationAddModel model, string userName, bool isApproved)
        {
            try
            {
                var registrationId = _db.Registrations.GetRegID_ByUserName(userName);
                var voucherNo = _db.Institutions.GetVoucherCountdown() + 1;

                _db.ExpenseTransportations.AddCustom(model, registrationId, voucherNo, isApproved);

                if (isApproved && model.AccountId != null) _db.Account.BalanceSubtract(model.AccountId.Value, model.TotalExpense);

                _db.Institutions.IncreaseVoucherCount();
                _db.SaveChanges();

                return new DbResponse(true, "Added Successfully");
            }
            catch (Exception e)
            {
                return new DbResponse(false, e.Message);
            }

        }

        public DbResponse ApprovedTransportationCost(int expenseTransportationId, int? accountId)
        {
            try
            {
                var amount = _db.ExpenseTransportations.Approved(expenseTransportationId, accountId);

                if (accountId != null) _db.Account.BalanceSubtract(accountId.Value, amount);

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
                var updateData = _db.ExpenseTransportations.Edit(model);

                if (updateData == null) return new DbResponse(false, "No data found");

                if (updateData.IsApproved)
                {
                    if (updateData.PrevAccountId != null)
                    {
                        _db.Account.BalanceAdd(updateData.PrevAccountId.Value, updateData.PrevAmount);
                    }

                    _db.SaveChanges();

                    if (updateData.CurrentAccountId != null)
                    {
                        _db.Account.BalanceSubtract(updateData.CurrentAccountId.Value, updateData.CurrentAmount);
                    }
                }
                _db.SaveChanges();
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

        public DbResponse<List<ExpenseCategoryWise>> CategoryWistSummaryDateToDate(DateTime? sDateTime, DateTime? eDateTime)
        {
            try
            {
                var data = _db.Expenses.CategoryWistSummaryDateToDate(sDateTime, eDateTime);
                return new DbResponse<List<ExpenseCategoryWise>>(true, "Success", data.ToList());
            }
            catch (Exception e)
            {
                return new DbResponse<List<ExpenseCategoryWise>>(false, e.Message);
            }
        }

        public DbResponse<List<ExpenseAllViewModel>> CategoryWistDetailsDateToDate(string category, DateTime? sDateTime, DateTime? eDateTime)
        {
            try
            {
                var data = _db.Expenses.CategoryWistDetailsDateToDate(category, sDateTime, eDateTime);
                return new DbResponse<List<ExpenseAllViewModel>>(true, "Success", data.ToList());
            }
            catch (Exception e)
            {
                return new DbResponse<List<ExpenseAllViewModel>>(false, e.Message);
            }
        }
    }
}