using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductCatalogRepository : Repository<ProductCatalog>, IProductCatalogRepository
    {
        public ProductCatalogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<DbResponse<ProductCatalogViewModel>> AddCustomAsync(ProductCatalogViewModel model)
        {
            var catalog = new ProductCatalog
            {
                CatalogTypeId = model.CatalogTypeId,
                CatalogName = model.CatalogName,
                CatalogLevel = 0,
                ParentId = model.ParentId
            };

            var response = new DbResponse<ProductCatalogViewModel>();

            await Context.ProductCatalog.AddAsync(catalog).ConfigureAwait(false);

            try
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);
                model.ProductCatalogId = catalog.ProductCatalogId;
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = model;
            }
            catch (DbUpdateException e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public IEnumerable<ProductCatalogShow> ListCustom()
        {

            var catalogs = Context.ProductCatalog
                .AsEnumerable()?
                .Where(x => x.Parent == null)
                .ToList()
                .Select(c => new ProductCatalogShow(c));

            return catalogs;
        }

        public ICollection<DDL> CatalogDll()
        {
            var ddls = Context.ProductCatalog
                .AsEnumerable()?
                .ToList()
                .Select(c => new DDL
                {
                    value = c.ProductCatalogId,
                    label =   CatalogDllFunction(c.Parent, c.CatalogName)
                });

            return ddls.ToList();
        }

        string CatalogDllFunction(ProductCatalog catalog, string cat)
        {
            
            if(catalog != null)
            {
                cat +=">";
                cat +=CatalogDllFunction(catalog.Parent, catalog.CatalogName);
            }


            return cat;
        }
    }
}
