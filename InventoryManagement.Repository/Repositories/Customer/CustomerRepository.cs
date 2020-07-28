using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
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

        public ICollection<CustomerListViewModel> ListCustom(bool customerType)
        {
            var cList = Context.Customer.Where(c => c.IsIndividual == customerType).Select(c => new CustomerListViewModel
            {
                CustomerId = c.CustomerId,
                OrganizationName = c.OrganizationName,
                CustomerName = c.CustomerName,
                CustomerAddress = c.CustomerAddress,
                PhonePrimary = c.CustomerPhone.FirstOrDefault(p => p.IsPrimary == true).Phone,
                Due = c.Due,
                DueLimit = c.DueLimit,
                Description = c.Description,
                Designation = c.Designation,
                IsIndividual = c.IsIndividual,
                SignUpDate = c.InsertDate
            });

            return cList.ToList();
        }

        public DataResult<CustomerListViewModel> ListDataTable(DataRequest request)
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
                Description = c.Description,
                Designation = c.Designation,
                IsIndividual = c.IsIndividual,
                SignUpDate = c.InsertDate
            });

            return cList.ToDataResult(request);
        }

        public async Task<bool> IsPhoneNumberExistAsync(string number, int id = 0)
        {
            if (id == 0)
                return await Context.CustomerPhone.AnyAsync(c => c.Phone == number).ConfigureAwait(false);

            return await Context.CustomerPhone.AnyAsync(c => c.Phone == number && c.CustomerId != id).ConfigureAwait(false);
        }

        public CustomerAddUpdateViewModel FindCustom(int id)
        {
            var customerList = Context.Customer.Include(c => c.CustomerPhone).Select(c => new CustomerAddUpdateViewModel
            {
                CustomerId = c.CustomerId,
                OrganizationName = c.OrganizationName,
                CustomerName = c.CustomerName,
                CustomerAddress = c.CustomerAddress,
                Description = c.Description,
                DueLimit = c.DueLimit,
                Photo = c.Photo,
                Designation = c.Designation,
                IsIndividual = c.IsIndividual,
                PhoneNumbers = c.CustomerPhone.Select(p => new CustomerPhoneViewModel
                {
                    CustomerPhoneId = p.CustomerPhoneId,
                    Phone = p.Phone,
                    IsPrimary = p.IsPrimary
                }).ToList(),

            });

            return customerList.FirstOrDefault(c => c.CustomerId == id);
        }

        public CustomerProfileViewModel ProfileDetails(int id)
        {
            var customer = Context.Customer
                .Include(c => c.Selling)
                .Select(c => new CustomerProfileViewModel
                {
                    CustomerId = c.CustomerId,
                    OrganizationName = c.OrganizationName,
                    CustomerName = c.CustomerName,
                    CustomerAddress = c.CustomerAddress,
                    Description = c.Description,
                    DueLimit = c.DueLimit,
                    Photo = c.Photo,
                    Designation = c.Designation,
                    IsIndividual = c.IsIndividual,
                    PhoneNumbers = c.CustomerPhone.Select(p => new CustomerPhoneViewModel
                    {
                        CustomerPhoneId = p.CustomerPhoneId,
                        Phone = p.Phone,
                        IsPrimary = p.IsPrimary
                    }).ToList(),
                    SellingRecords = c.Selling.Select(s => new CustomerSellingViewModel
                    {
                        SellingId = s.SellingId,
                        SellingSn = s.SellingSn,
                        SellingAmount = s.SellingTotalPrice,
                        SellingPaidAmount = s.SellingPaidAmount,
                        SellingDiscountAmount = s.SellingDiscountAmount,
                        SellingDueAmount = s.SellingDueAmount,
                        SellingDate = s.SellingDate
                    }).ToList(),
                    SignUpDate = c.InsertDate,
                    SoldAmount = c.TotalAmount,
                    DiscountAmount = c.TotalDiscount,
                    DueAmount = c.Due,
                    ReceivedAmount = c.Paid
                });
            return customer.FirstOrDefault(c => c.CustomerId == id);
        }

        public void CustomUpdate(CustomerAddUpdateViewModel model)
        {
            var customer = Context.Customer.Include(c => c.CustomerPhone).FirstOrDefault(c => c.CustomerId == model.CustomerId);
            if (customer == null) return;
            if (model.Photo != null && model.Photo.Length > 0)
                customer.Photo = model.Photo;

            customer.CustomerAddress = model.CustomerAddress;
            customer.CustomerName = model.CustomerName;
            customer.OrganizationName = model.OrganizationName;
            customer.Description = model.Description;
            customer.DueLimit = model.DueLimit;
            customer.Designation = model.Designation;
            customer.IsIndividual = model.IsIndividual;
            customer.CustomerPhone = model.PhoneNumbers.Select(p => new CustomerPhone
            {
                CustomerPhoneId = p.CustomerPhoneId.GetValueOrDefault(),
                Phone = p.Phone,
                IsPrimary = p.IsPrimary ?? false,
                InsertDate = DateTime.Now
            }).ToList();
            Context.Customer.Update(customer);
        }

        public void UpdatePaidDue(int id)
        {
            var customer = Find(id);

            var obj = Context.Selling.Where(s => s.CustomerId == customer.CustomerId).GroupBy(s => s.CustomerId).Select(s =>
                new
                {
                    TotalAmount = s.Sum(c => c.SellingTotalPrice),
                    TotalDiscount = s.Sum(c => c.SellingDiscountAmount),
                    Paid = s.Sum(c => c.SellingPaidAmount)
                }).FirstOrDefault();

            customer.TotalAmount = obj.TotalAmount;
            customer.TotalDiscount = obj.TotalDiscount;
            customer.Paid = obj.Paid;

            Update(customer);
            Context.SaveChanges();
        }

        public async Task<ICollection<CustomerListViewModel>> SearchAsync(string key)
        {
            return await Context.Customer.Include(c => c.CustomerPhone)
                .Where(c => c.CustomerName.Contains(key) || c.CustomerPhone.Any(p => p.Phone.Contains(key)) || c.OrganizationName.Contains(key))
                .Select(c =>
                  new CustomerListViewModel
                  {
                      CustomerId = c.CustomerId,
                      OrganizationName = c.OrganizationName,
                      CustomerName = c.CustomerName,
                      CustomerAddress = c.CustomerAddress,
                      PhonePrimary = c.CustomerPhone.FirstOrDefault(p => p.Phone.Contains(key)).Phone ?? c.CustomerPhone.FirstOrDefault(p=> p.IsPrimary == true).Phone,
                      Due = c.Due,
                      DueLimit = c.DueLimit,
                      SignUpDate = c.InsertDate,
                      Designation = c.Designation,
                      IsIndividual = c.IsIndividual,
                      Description = c.Description
                  }).Take(5).ToListAsync().ConfigureAwait(false);
        }

        public double TotalDue()
        {
            return Context.Customer?.Sum(s => s.Due) ?? 0;
        }

        public ICollection<CustomerDueViewModel> TopDue(int totalCustomer)
        {
            return Context.Customer.Where(c => c.Due > 0).OrderByDescending(c => c.Due).Take(totalCustomer).Select(c => new CustomerDueViewModel
            {
                CustomerId = c.CustomerId,
                Name = c.CustomerName,
                Due = c.Due
            }).ToList();
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
                IsIndividual = model.IsIndividual,
                Designation = model.Designation,
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