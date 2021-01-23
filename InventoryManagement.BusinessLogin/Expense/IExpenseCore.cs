using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;

namespace InventoryManagement.BusinessLogin
{
    public interface IExpenseCore
    {
        DbResponse AddTransportationCost(ExpenseTransportationAddModel model, string userName, bool isApproved);
        DbResponse ApprovedTransportationCost(int expenseTransportationId);
        DbResponse DeleteTransportationCost(int expenseTransportationId);
        DbResponse<ExpenseTransportationDetailsModel> GetTransportationCostDetails(int expenseTransportationId);
        DbResponse EditTransportationCost(ExpenseTransportationDetailsModel model);

        DbResponse AddFixedCost(ExpenseFixedAddModel model);
        DbResponse DeleteFixedCost(int expenseFixedId);
        DbResponse<DataResult<ExpenseFixedViewModel>> FixedCostRecords(DataRequest request);


    }
}