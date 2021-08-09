using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface ICustomerRepository : IRepository<Customer>, IAddCustom<CustomerAddUpdateViewModel>
    {
        ICollection<CustomerListViewModel> ListCustom(bool customerType);
        DataResult<CustomerListViewModel> ListDataTable(DataRequest request);
        Task<bool> IsPhoneNumberExistAsync(string number, int id = 0);
        CustomerAddUpdateViewModel FindCustom(int id);
        CustomerProfileViewModel ProfileDetails(int id);
        DataResult<CustomerSellingViewModel> SellingRecord(DataRequest request);
        void CustomUpdate(CustomerAddUpdateViewModel model);
        void UpdatePaidDue(int id);
        Task<ICollection<CustomerListViewModel>> SearchAsync(string key);
        decimal TotalDue();
        ICollection<CustomerDueViewModel> TopDue(int totalCustomer);
        DataResult<CustomerDueViewModel> TopDueDataTable(DataRequest request);

        bool IsDueLimitCrossed(int customerId, decimal newDue);
    }
}