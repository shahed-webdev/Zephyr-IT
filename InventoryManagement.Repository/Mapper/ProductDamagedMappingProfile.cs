using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class ProductDamagedMappingProfile : Profile
    {
        public ProductDamagedMappingProfile()
        {
            CreateMap<ProductDamagedAddModel, ProductDamaged>();
            CreateMap<ProductDamaged, ProductDamagedViewModel>()
                .ForMember(d => d.ProductCode, opt => opt.MapFrom(c => c.ProductStock.ProductCode))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(c => c.ProductStock.Product.ProductName))
                .ForMember(d => d.ProductDescription, opt => opt.MapFrom(c => c.ProductStock.Product.Description))
                .ForMember(d => d.DamagedAmount, opt => opt.MapFrom(c => c.ProductStock.PurchaseList.PurchasePrice))

                ;
        }
    }
}