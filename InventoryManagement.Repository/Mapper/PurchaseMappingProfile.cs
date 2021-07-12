using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class PurchaseMappingProfile : Profile
    {
        public PurchaseMappingProfile()
        {
            CreateMap<PurchasePayment, PurchasePaymentRecordViewModel>()
                .ForMember(d => d.VendorCompanyName, opt => opt.MapFrom(c => c.Vendor.VendorCompanyName))
                .ForMember(d => d.AccountName, opt => opt.MapFrom(c => c.Account.AccountName));
        }
    }
}