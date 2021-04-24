using AutoMapper;
using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public class ProductLogMappingProfile : Profile
    {
        public ProductLogMappingProfile()
        {
            CreateMap<ProductLogAddModel, ProductLog>();
        }
    }
}