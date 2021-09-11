using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<AccountCrudModel, Account>().ReverseMap();
            CreateMap<AccountDepositCrudModel, AccountDeposit>().ReverseMap();
            CreateMap<AccountWithdrawCrudModel, AccountWithdraw>().ReverseMap();
            CreateMap<SellingPaymentReturnRecordModel, SellingPaymentReturnRecord>().ReverseMap();
            CreateMap<PurchasePaymentReturnRecordModel, PurchasePaymentReturnRecord>().ReverseMap();

        }
    }
}