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
            CreateMap<SellingExpense, SellingExpenseListModel>()
                .ForMember(d => d.AccountName, opt => opt.MapFrom(c => c.Account.AccountName))
                .ReverseMap();
            CreateMap<Selling, SellingBillProfitModel>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(c => c.Customer.CustomerName))
                ;


            CreateMap<Selling, CustomerSellingViewModel>();

        }
    }
}
