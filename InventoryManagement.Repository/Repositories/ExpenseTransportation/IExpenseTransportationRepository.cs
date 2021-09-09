using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public interface IExpenseTransportationRepository : IRepository<ExpenseTransportation>
    {
        void AddCustom(ExpenseTransportationAddModel model, int reregistrationId, int voucherNo, bool isApproved);
        decimal Approved(int expenseTransportationId, int? accountId);
        void Delete(int expenseTransportationId);
        ExpenseTransportationDetailsModel GetDetails(int expenseTransportationId);
        ExpenseUpdateAccountUpdateModel Edit(ExpenseTransportationDetailsModel model);
    }
}