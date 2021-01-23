using InventoryManagement.Data;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IExpenseTransportationRepository : IRepository<ExpenseTransportation>
    {
        void AddCustom(ExpenseTransportationAddModel model, int reregistrationId, int voucherNo, bool isApproved);
        void Approved(int expenseTransportationId);
        void Delete(int expenseTransportationId);
        ExpenseTransportationDetailsModel GetDetails(int expenseTransportationId);
        void Edit(ExpenseTransportationDetailsModel model);
    }
}