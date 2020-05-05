﻿using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductCatalogTypeRepository : Repository<ProductCatalogType>, IProductCatalogTypeRepository
    {
        public ProductCatalogTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddCustomAsync(ProductCatalogTypeViewModel model)
        {
            var catalogType = new ProductCatalogType
            {
                CatalogType = model.CatalogType
            };

            await Context.AddAsync(catalogType).ConfigureAwait(false);
        }


        public Task<List<ProductCatalogTypeViewModel>> ListCustomAsync()
        {
            return Context.ProductCatalogType.Select(c => new ProductCatalogTypeViewModel
            {
                CatalogTypeId = c.CatalogTypeId,
                CatalogType = c.CatalogType
            }).ToListAsync();
        }

        public Task DeleteCustomAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateCustomAsync(ProductCatalogTypeViewModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}