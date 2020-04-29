using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public interface ICustomerRepository : IRepository<Customer>, IAddCustom<CustomerAddUpdateViewModel>
    {
        ICollection<CustomerListViewModel> ListCustom();
        Task<bool> IsPhoneNumberExistAsync(string number, int id = 0);
        CustomerAddUpdateViewModel FindCustom(int id);

        void CustomUpdate(CustomerAddUpdateViewModel model);
    }
}