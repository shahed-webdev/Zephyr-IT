using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System.Collections.Generic;

namespace InventoryManagement.BusinessLogin
{
    public interface IExpenseCore
    {

        DbResponse<DataResult<ExpenseAllViewModel>> ExpenseRecords(DataRequest request);
        DbResponse<List<ExpenseAllViewModel>> ExpenseRecords();

        //DbResponse AddCost(ExpenseAddModel model, string userName, bool isApproved);
        //DbResponse ApprovedCost(int expenseId);
        //DbResponse DeleteCost(int expenseId);
        //DbResponse EditCost(ExpenseDetailsModel model);



        DbResponse AddTransportationCost(ExpenseTransportationAddModel model, string userName, bool isApproved);
        DbResponse ApprovedTransportationCost(int expenseTransportationId);
        DbResponse DeleteTransportationCost(int expenseTransportationId);
        DbResponse<ExpenseTransportationDetailsModel> GetTransportationCostDetails(int expenseTransportationId);
        DbResponse EditTransportationCost(ExpenseTransportationDetailsModel model);


        DbResponse<ExpenseFixedViewModel> AddFixedCost(ExpenseFixedAddModel model);
        DbResponse DeleteFixedCost(int expenseFixedId);
        DbResponse<List<ExpenseFixedViewModel>> FixedCostRecords();


    }
}