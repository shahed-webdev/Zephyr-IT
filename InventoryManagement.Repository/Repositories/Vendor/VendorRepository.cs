using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(ApplicationDbContext context) : base(context)
        {
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
                VendorPhone = model.VendorPhone
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
                Due = vendor.Due

            };
        }

        public bool RemoveCustom(int id)
        {
            //if (Context.Selling.Any(s => s.VendorID == id)) return false;
            Remove(Find(id));
            return true;
        }
    }
}
