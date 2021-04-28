using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class AdminMoneyCollectionMappingProfile : Profile
    {
        public AdminMoneyCollectionMappingProfile()
        {
            CreateMap<AdminMoneyCollectionAddModel, AdminMoneyCollection>();
            CreateMap<AdminMoneyCollection, AdminMoneyCollectionViewModel>()
                .ForMember(d => d.CollectionFrom, opt => opt.MapFrom(c => $"{c.Registration.Name} ({c.Registration.UserName})"))
                .ForMember(d => d.InsertDate, opt => opt.MapFrom(c => c.InsertDateUtc))

                ;
        }
    }
}