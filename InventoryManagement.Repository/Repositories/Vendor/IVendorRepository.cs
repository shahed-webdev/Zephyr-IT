using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task<ICollection<VendorViewModel>> ToListCustomAsync();
        DataResult<VendorViewModel> ToListDataTable(DataRequest request);
        Task<ICollection<VendorViewModel>> SearchAsync(string key);
        Vendor AddCustom(VendorViewModel model);
        void UpdateCustom(VendorViewModel model);
        VendorViewModel FindCustom(int? id);
        VendorProfileViewModel ProfileDetails(int id, DateTime? fromDate, DateTime? toDate);
        void UpdatePaidDue(int id);
        bool RemoveCustom(int id);
        bool IsPhoneExist(string number);
        decimal TotalDue();
        ICollection<VendorPaidDue> TopDue(int totalVendor);
        DataResult<VendorPaidDue> TopDueDataTable(DataRequest request);
    }

}

