using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;

namespace InventoryManagement.BusinessLogin
{
    public interface IExpenseCore
    {

        DbResponse<DataResult<ExpenseAllViewModel>> ExpenseRecords(DataRequest request);
        DbResponse<List<ExpenseAllViewModel>> ExpenseRecords();

        DbResponse AddCost(ExpenseAddModel model, string userName, bool isApproved);
        DbResponse ApprovedCost(int expenseId, int? accountId);
        DbResponse DeleteCost(int expenseId);
        DbResponse EditCost(ExpenseAddModel model);

        DbResponse<ExpenseDetailsModel> GetCost(int expenseId);

        DbResponse AddTransportationCost(ExpenseTransportationAddModel model, string userName, bool isApproved);
        DbResponse ApprovedTransportationCost(int expenseTransportationId, int? accountId);
        DbResponse DeleteTransportationCost(int expenseTransportationId);
        DbResponse<ExpenseTransportationDetailsModel> GetTransportationCostDetails(int expenseTransportationId);
        DbResponse EditTransportationCost(ExpenseTransportationDetailsModel model);


        DbResponse<ExpenseFixedViewModel> AddFixedCost(ExpenseFixedAddModel model);
        DbResponse DeleteFixedCost(int expenseFixedId);
        DbResponse<List<ExpenseFixedViewModel>> FixedCostRecords();
        DbResponse<List<ExpenseCategoryWise>> CategoryWistSummaryDateToDate(DateTime? sDateTime, DateTime? eDateTime);
        DbResponse<List<ExpenseAllViewModel>> CategoryWistDetailsDateToDate(string category, DateTime? sDateTime, DateTime? eDateTime);

    }
}