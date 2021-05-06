using AutoMapper;
using InventoryManagement.Data;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class WarrantyMappingProfile : Profile
    {
        public WarrantyMappingProfile()
        {
            CreateMap<WarrantyAcceptanceModel, Warranty>();
            CreateMap<Warranty, WarrantyReceiptModel>()
                .ForMember(d => d.ProductCode, opt => opt.MapFrom(c => c.ProductStock.ProductCode))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(c => c.Selling.Customer.CustomerName))
                .ForMember(d => d.CustomerPhone, opt => opt.MapFrom(c => c.Selling.Customer.CustomerPhone.FirstOrDefault().Phone))
                .ForMember(d => d.ProductCatalogName, opt => opt.MapFrom(c => c.ProductStock.Product.ProductCatalog.CatalogName))
                .ForMember(d => d.ChangedProductCatalogName, opt => opt.MapFrom(c => c.ProductCatalog.CatalogName))
                .ForMember(d => d.ProductDescription, opt => opt.MapFrom(c => c.ProductStock.Product.Description))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(c => c.ProductStock.Product.ProductName))
                .ForMember(d => d.SellingSn, opt => opt.MapFrom(c => c.Selling.SellingSn))
                .ForMember(d => d.SellingDate, opt => opt.MapFrom(c => c.Selling.SellingDate))
                ;

            CreateMap<Warranty, WarrantyListViewModel>()
                .ForMember(d => d.ProductCode, opt => opt.MapFrom(c => c.ProductStock.ProductCode))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(c => c.Selling.Customer.CustomerName))
                .ForMember(d => d.CustomerPhone, opt => opt.MapFrom(c => c.Selling.Customer.CustomerPhone.FirstOrDefault().Phone))
                .ForMember(d => d.ProductCatalogName, opt => opt.MapFrom(c => c.ProductStock.Product.ProductCatalog.CatalogName))
                .ForMember(d => d.ProductDescription, opt => opt.MapFrom(c => c.ProductStock.Product.Description))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(c => c.ProductStock.Product.ProductName))
                .ForMember(d => d.SellingSn, opt => opt.MapFrom(c => c.Selling.SellingSn))
                .ForMember(d => d.SellingDate, opt => opt.MapFrom(c => c.Selling.SellingDate))
                ;
        }
    }
}