using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class VendorMappingProfile : Profile
    {
        public VendorMappingProfile()
        {
            CreateMap<Vendor, VendorProfileViewModel>();
        }

    }
}