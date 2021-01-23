using InventoryManagement.Repository;
using System.Collections.Generic;

namespace InventoryManagement.BusinessLogin
{
    public interface IExpenseCore
    {
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