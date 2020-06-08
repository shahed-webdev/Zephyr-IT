using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ICollection<CustomerListViewModel> ListCustom()
        {
            var cList = Context.Customer.Select(c => new CustomerListViewModel
            {
                CustomerId = c.CustomerId,
                OrganizationName = c.OrganizationName,
                CustomerName = c.CustomerName,
                CustomerAddress = c.CustomerAddress,
                PhonePrimary = c.CustomerPhone.FirstOrDefault(p => p.IsPrimary == true).Phone,
                Due = c.Due,
                DueLimit = c.DueLimit,
                SignUpDate = c.InsertDate
            });

            return cList.ToList();
        }

        public async Task<bool> IsPhoneNumberExistAsync(string number, int id = 0)
        {
            if (id == 0)
                return await Context.CustomerPhone.AnyAsync(c => c.Phone == number).ConfigureAwait(false);

            return await Context.CustomerPhone.AnyAsync(c => c.Phone == number && c.CustomerId != id).ConfigureAwait(false);
        }

        public CustomerAddUpdateViewModel FindCustom(int id)
        {
            var customerList = Context.Customer.Select(c => new CustomerAddUpdateViewModel
            {
                CustomerId = c.CustomerId,
                OrganizationName = c.OrganizationName,
                CustomerName = c.CustomerName,
                CustomerAddress = c.CustomerAddress,
                Description = c.Description,
                DueLimit = c.DueLimit,
                Photo = c.Photo,
                PhoneNumbers = c.CustomerPhone.Select(p => new CustomerPhoneViewModel
                {
                    CustomerPhoneId = p.CustomerPhoneId,
                    Phone = p.Phone,
                    IsPrimary = p.IsPrimary
                }).ToList(),

            });

            return customerList.FirstOrDefault(c => c.CustomerId == id);
        }

        public void CustomUpdate(CustomerAddUpdateViewModel model)
        {
            var customer = Find(model.CustomerId);
            if (model.Photo != null && model.Photo.Length > 0)
                customer.Photo = model.Photo;

            customer.CustomerAddress = model.CustomerAddress;
            customer.CustomerName = model.CustomerName;
            customer.OrganizationName = model.OrganizationName;
            customer.Description = model.Description;
            customer.DueLimit = model.DueLimit;
            Update(customer);

            foreach (var item in model.PhoneNumbers.Where(p => p.CustomerPhoneId != 0))
            {
                var phone = Context.CustomerPhone.Find(item.CustomerPhoneId);
                phone.Phone = item.Phone;
                phone.IsPrimary = item.IsPrimary ?? false;
                Context.CustomerPhone.Update(phone);
            }

            var newPhones = model.PhoneNumbers.Where(p => p.CustomerPhoneId == 0).Select(p => new CustomerPhone
            {
                CustomerId = model.CustomerId,
                Phone = p.Phone,
                IsPrimary = p.IsPrimary ?? false
            });

            Context.CustomerPhone.AddRange(newPhones);
        }

        public async Task<ICollection<CustomerListViewModel>> SearchAsync(string key)
        {
            return await Context.Customer.Where(c => c.CustomerName.Contains(key) || c.CustomerPhone.Select(p => p.Phone).Contains(key) || c.OrganizationName.Contains(key)).Select(c =>
                  new CustomerListViewModel
                  {
                      CustomerId = c.CustomerId,
                      OrganizationName = c.OrganizationName,
                      CustomerName = c.CustomerName,
                      CustomerAddress = c.CustomerAddress,
                      PhonePrimary = c.CustomerPhone.FirstOrDefault(p => p.IsPrimary.GetValueOrDefault()).Phone,
                      Due = c.Due,
                      DueLimit = c.DueLimit,
                      SignUpDate = c.InsertDate
                  }).Take(5).ToListAsync().ConfigureAwait(false);
        }


        public void AddCustom(CustomerAddUpdateViewModel model)
        {
            var customer = new Customer
            {
                OrganizationName = model.OrganizationName,
                CustomerName = model.CustomerName,
                CustomerAddress = model.CustomerAddress,
                Description = model.Description,
                DueLimit = model.DueLimit,
                CustomerPhone = model.PhoneNumbers.Select(p => new CustomerPhone
                {
                    Phone = p.Phone,
                    IsPrimary = p.IsPrimary
                }).ToList()
            };

            if (model.Photo != null && model.Photo.Length > 0)
                customer.Photo = model.Photo;


            Add(customer);
        }
    }
}