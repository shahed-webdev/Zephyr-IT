using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ICollection<CustomerListViewModel> ListCustom()
        {
            var cList =  Context.Customer.Select(c => new CustomerListViewModel
            {
                CustomerId = c.CustomerId,
                OrganizationName = c.OrganizationName,
                CustomerName = c.CustomerName,
                CustomerAddress =c.CustomerAddress,
                PhonePrimary = c.CustomerPhone.FirstOrDefault(p=> p.IsPrimary.GetValueOrDefault()).Phone,
                Due = c.Due,
                SignUpDate = c.InsertDate
            });

            return cList.ToList();
        }

        public async Task<bool> IsPhoneNumberExistAsync(string number, int id = 0)
        {
            if(id == null)
            {
              return  await Context.CustomerPhone.AnyAsync(c => c.Phone == number);
            }
            else
            {
                return await Context.CustomerPhone.AnyAsync(c => c.Phone == number | c.CustomerId != id.GetValueOrDefault());
            }
        }

        public CustomerAddUpdateViewModel FindCustom(int id)
        {
            var cList = Context.Customer.Select(c => new CustomerAddUpdateViewModel
            {

                CustomerId = c.CustomerId,
                OrganizationName = c.OrganizationName,
                CustomerName = c.CustomerName,
                CustomerAddress = c.CustomerAddress, 
                Photo = c.Photo, 
                PhoneNumbers = c.CustomerPhone.Select(p => new CustomerPhoneViewModel
                {
                    CustomerPhoneId = p.CustomerPhoneId,
                    Phone = p.Phone, 
                    IsPrimary = p.IsPrimary
                }).ToList(),

            });

            return cList.FirstOrDefault(c=> c.CustomerId == id);
        }

        public void CustomUpdate(CustomerAddUpdateViewModel model)
        {
            var customer = Find(model.CustomerId);

            customer.Photo = model.Photo;
            customer.CustomerAddress = model.CustomerAddress;
            customer.CustomerName = model.CustomerName;
          customer.OrganizationName = model.OrganizationName;
          customer.CustomerPhone = model.PhoneNumbers.Select(p => new CustomerPhone
          {
              CustomerPhoneId = p.CustomerPhoneId,
              Phone = p.Phone,
              IsPrimary = p.IsPrimary
              
          }).ToList();

           Update(customer);
         
        }


        public void AddCustom(CustomerAddUpdateViewModel model)
        {
            var customer = new Customer
            {
                OrganizationName = model.OrganizationName,
                CustomerName = model.CustomerName,
                CustomerAddress = model.CustomerAddress,
                CustomerPhone = model.PhoneNumbers.Select(p => new CustomerPhone
                {
                    Phone = p.Phone,
                    IsPrimary = p.IsPrimary
                }).ToList()
            };

            if (model.Photo != null && model.Photo.Length > 0)
            {
                customer.Photo = model.Photo;
            }

            Add(customer);
        }
    }
}