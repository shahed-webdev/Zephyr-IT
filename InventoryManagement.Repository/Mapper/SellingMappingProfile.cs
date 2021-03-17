using AutoMapper;
using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Repository 
{
    class SellingMappingProfile : Profile
    {
        public SellingMappingProfile()
        {
            //CreateMap<SellingViewModel, Selling>();
            //CreateMap<SellingProductListViewModel, SellingList>();
            CreateMap<SellingExpense, SellingExpenseListModel>().ReverseMap();

        }
    }
}
