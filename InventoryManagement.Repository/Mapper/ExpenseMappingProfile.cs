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
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(c => c.Customer.CustomerName))
                .ForMember(d => d.AccountName, opt => opt.MapFrom(c => c.Account.AccountName))
                .ForMember(d => d.CreateBy, opt => opt.MapFrom(c => c.Registration.UserName));

            CreateMap<ExpenseFixedAddModel, ExpenseFixed>();
            CreateMap<ExpenseFixed, ExpenseFixedViewModel>();

            CreateMap<Expense, ExpenseAllViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(c => c.ExpenseId))
                .ForMember(d => d.CreateBy, opt => opt.MapFrom(c => c.Registration.UserName))
                .ForMember(d => d.ExpenseCategory, opt => opt.MapFrom(c => c.ExpenseCategory.CategoryName))
                .ForMember(d => d.IsTransportation, opt => opt.MapFrom(c => false))
                .ForMember(d => d.AccountName, opt => opt.MapFrom(c => c.Account.AccountName));

            CreateMap<ExpenseTransportation, ExpenseAllViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(c => c.ExpenseTransportationId))
                .ForMember(d => d.CreateBy, opt => opt.MapFrom(c => c.Registration.UserName))
                .ForMember(d => d.ExpenseCategory, opt => opt.MapFrom(c => "Transportation"))
                .ForMember(d => d.ExpenseFor, opt => opt.MapFrom(c => c.ExpenseNote))
                .ForMember(d => d.ExpenseAmount, opt => opt.MapFrom(c => c.TotalExpense))
                .ForMember(d => d.AccountName, opt => opt.MapFrom(c => c.Account.AccountName))
                .ForMember(d => d.IsTransportation, opt => opt.MapFrom(c => true));

            CreateMap<VW_ExpenseWithTransportation, ExpenseAllViewModel>();
        }
    }
}