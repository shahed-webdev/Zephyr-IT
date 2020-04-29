using System;
using System.Collections.Generic;
using System.Text;
using InventoryManagement.Data;

namespace InventoryManagement.Repository 
{
    public class CustomerRepository : Repository<Customer>,ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
