using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class ProductLogMappingProfile : Profile
    {
        public ProductLogMappingProfile()
        {
            CreateMap<ProductLogAddModel, ProductLog>();
            CreateMap<ProductLog, ProductLogViewModel>()
                .ForMember(d => d.ActivityBy, opt => opt.MapFrom(c => $"{c.Registration.Name} ({c.Registration.UserName})"))
                ;
        }
    }
}