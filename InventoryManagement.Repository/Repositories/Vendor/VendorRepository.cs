using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
