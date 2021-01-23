using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class ExpenseMappingProfile : Profile
    {
        public ExpenseMappingProfile()
        {
            CreateMap<ExpenseTransportationAddModel, ExpenseTransportation>();
            CreateMap<ExpenseTransportationListAddModel, ExpenseTransportationList>().ReverseMap();

            CreateMap<ExpenseTransportation, ExpenseTransportationDetailsModel>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(c => c.Customer.CustomerName));

            CreateMap<ExpenseFixedAddModel, ExpenseFixed>();
            CreateMap<ExpenseFixed, ExpenseFixedViewModel>();
        }
    }
}