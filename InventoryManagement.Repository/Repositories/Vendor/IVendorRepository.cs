using InventoryManagement.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task<ICollection<VendorViewModel>> ToListCustomAsync();
        Task<ICollection<VendorViewModel>> SearchAsync(string key);
        Vendor AddCustom(VendorViewModel model);
        void UpdateCustom(VendorViewModel model);
        VendorViewModel FindCustom(int? id);

        public bool RemoveCustom(int id);
    }

}

