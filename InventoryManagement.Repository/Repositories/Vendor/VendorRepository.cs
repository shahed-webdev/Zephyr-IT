using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        private readonly IMapper _mapper;

        public VendorRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<ICollection<VendorViewModel>> ToListCustomAsync()
        {
            var vendor = await Context.Vendor.Select(v => new VendorViewModel
            {
                VendorId = v.VendorId,
                VendorCompanyName = v.VendorCompanyName,
                VendorName = v.VendorName,
                VendorAddress = v.VendorAddress,
                VendorPhone = v.VendorPhone,
                InsertDate = v.InsertDate,
                Due = v.Due
            }).ToListAsync().ConfigureAwait(false);

            return vendor;
        }

        public DataResult<VendorViewModel> ToListDataTable(DataRequest request)
        {
            var vendor = Context.Vendor.Select(v => new VendorViewModel
            {
                VendorId = v.VendorId,
                VendorCompanyName = v.VendorCompanyName,
                VendorName = v.VendorName,
                VendorAddress = v.VendorAddress,
                VendorPhone = v.VendorPhone,
                InsertDate = v.InsertDate,
                Due = v.Due
            });

            return vendor.ToDataResult(request);
        }

        public async Task<ICollection<VendorViewModel>> SearchAsync(string key)
        {
            return await Context.Vendor.Where(v => v.VendorName.Contains(key) || v.VendorPhone.Contains(key) || v.VendorCompanyName.Contains(key)).Select(v =>
                new VendorViewModel
                {
                    VendorId = v.VendorId,
                    VendorCompanyName = v.VendorCompanyName,
                    VendorName = v.VendorName,
                    VendorAddress = v.VendorAddress,
                    VendorPhone = v.VendorPhone,
                    InsertDate = v.InsertDate,
                    Due = v.Due
                }).Take(5).ToListAsync().ConfigureAwait(false);
        }

        public Vendor AddCustom(VendorViewModel model)
        {
            var vendor = new Vendor
            {
                VendorCompanyName = model.VendorCompanyName,
                VendorName = model.VendorName,
                VendorAddress = model.VendorAddress,
                VendorPhone = model.VendorPhone,
                Description = model.Description
            };

            Add(vendor);

            return vendor;
        }

        public void UpdateCustom(VendorViewModel model)
        {
            var vendor = Find(model.VendorId);

            vendor.VendorCompanyName = model.VendorCompanyName;
            vendor.VendorName = model.VendorName;
            vendor.VendorAddress = model.VendorAddress;
            vendor.VendorPhone = model.VendorPhone;
            vendor.Description = model.Description;

            Update(vendor);
        }

        public VendorViewModel FindCustom(int? id)
        {
            var vendor = Find(id.GetValueOrDefault());
            if (vendor == null) return null;

            return new VendorViewModel
            {
                VendorId = vendor.VendorId,
                VendorCompanyName = vendor.VendorCompanyName,
                VendorName = vendor.VendorName,
                VendorAddress = vendor.VendorAddress,
                VendorPhone = vendor.VendorPhone,
                InsertDate = vendor.InsertDate,
                Description = vendor.Description,
                Due = vendor.Due
            };
        }

        public VendorProfileViewModel ProfileDetails(int id, DateTime? fromDate, DateTime? toDate)
        {
            var sD = fromDate ?? new DateTime(1000, 1, 1);
            var eD = toDate ?? new DateTime(3000, 12, 31);
            var vendor = Context.Vendor
                .ProjectTo<VendorProfileViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault(v => v.VendorId == id);

            var sum = Context.Purchase
                .Where(p => p.VendorId == id && p.PurchaseDate <= eD && p.PurchaseDate >= sD)
                .GroupBy(s => true)
                .Select(g => new
                {
                    Discount = g.Sum(e => e.PurchaseDiscountAmount),
                    TotalPrice = g.Sum(e => e.PurchaseTotalPrice),
                    Due = g.Sum(e => e.PurchaseDueAmount)
                }).FirstOrDefault();

            var paid = Context.PurchasePayment
                .Where(p => p.VendorId == id && p.PaidDate <= eD && p.PaidDate >= sD).Sum(p => p.PaidAmount);
            vendor.Due = sum.Due;
            vendor.Paid = paid;
            vendor.TotalAmount = sum.TotalPrice;
            vendor.TotalDiscount = sum.Discount;

            return vendor;
        }

        public void UpdatePaidDue(int id)
        {
            var vendor = Find(id);
            var obj = Context.Purchase.Where(p => p.VendorId == vendor.VendorId).GroupBy(pg => pg.VendorId).Select(p =>
                new
                {
                    TotalAmount = p.Sum(c => c.PurchaseTotalPrice),
                    TotalDiscount = p.Sum(c => c.PurchaseDiscountAmount),
                    Paid = p.Sum(c => c.PurchasePaidAmount),
                    Return = p.Sum(c => c.PurchaseReturnAmount)
                }).FirstOrDefault();

            vendor.TotalAmount = obj.TotalAmount;
            vendor.TotalDiscount = obj.TotalDiscount;
            vendor.Paid = obj.Paid;
            vendor.ReturnAmount = obj.Return;

            Update(vendor);
            Context.SaveChanges();
        }

        public bool RemoveCustom(int id)
        {
            if (Context.Purchase.Any(s => s.VendorId == id)) return false;
            Remove(Find(id));
            return true;
        }

        public bool IsPhoneExist(string number)
        {
            return Context.Vendor.Any(v => v.VendorPhone == number);
        }

        public decimal TotalDue()
        {
            return Context.Vendor?.Sum(v => v.Due) ?? 0;
        }

        public ICollection<VendorPaidDue> TopDue(int totalVendor)
        {
            return Context.Vendor.Where(v => v.Due > 0).OrderByDescending(v => v.Due).Take(totalVendor).Select(v => new VendorPaidDue
            {
                VendorId = v.VendorId,
                VendorCompanyName = v.VendorCompanyName,
                VendorDue = v.Due,
                VendorPaid = v.Paid,
                TotalAmount = v.TotalAmount,
                TotalDiscount = v.TotalDiscount
            }).ToList();
        }

        public DataResult<VendorPaidDue> TopDueDataTable(DataRequest request)
        {
            return Context.Vendor.Where(v => v.Due > 0)
                .OrderByDescending(v => v.Due).Select(v => new VendorPaidDue
                {
                    VendorId = v.VendorId,
                    VendorCompanyName = v.VendorCompanyName,
                    VendorDue = v.Due,
                    VendorPaid = v.Paid,
                    TotalAmount = v.TotalAmount,
                    TotalDiscount = v.TotalDiscount
                }).ToDataResult(request);
        }
    }
}
