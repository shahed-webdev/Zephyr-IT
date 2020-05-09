using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            var response = new DbResponse<ProductCatalogViewModel>();

            if (Context.ProductCatalog.Any(c => c.ParentId == model.ParentId && c.CatalogName == model.CatalogName))
            {
                response.Message = "Name already exists";
                response.IsSuccess = false;
                return response;
            }

            var level = 0;
            if (model.ParentId != null)
            {
                level = Context.ProductCatalog.Find(model.ParentId).CatalogLevel + 1;
            }

            var catalog = new ProductCatalog
            {
                CatalogTypeId = model.CatalogTypeId,
                CatalogName = model.CatalogName,
                CatalogLevel = level,
                ParentId = model.ParentId
            };



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
                    label = CatalogDllFunction(c.Parent, c.CatalogName)
                });

            return ddls.ToList();
        }

        string CatalogDllFunction(ProductCatalog catalog, string cat)
        {

            if (catalog != null)
            {
                cat += ">";
                cat += CatalogDllFunction(catalog.Parent, catalog.CatalogName);
            }


            return cat;
        }
    }
}
