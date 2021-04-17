using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    class SellingMappingProfile : Profile
    {
        public SellingMappingProfile()
        {
            //CreateMap<SellingViewModel, Selling>();
            //CreateMap<SellingProductListViewModel, SellingList>();
            CreateMap<SellingExpense, SellingExpenseListModel>().ReverseMap();
            CreateMap<Selling, SellingBillProfitModel>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(c => c.Customer.CustomerName))
                ;


            CreateMap<Selling, CustomerSellingViewModel>();

        }
    }
}
