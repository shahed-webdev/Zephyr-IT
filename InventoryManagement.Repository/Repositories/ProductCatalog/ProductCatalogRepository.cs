using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
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
                .ToList().OrderBy(c => c.ParentId).ThenBy(c => c.CatalogLevel).ThenBy(c => c.CatalogName)
                .Select(c => new DDL
                {
                    value = c.ProductCatalogId,
                    label = CatalogDllFunction(c.Parent, c.CatalogName)
                });

            return ddls.ToList();
        }

        public string CatalogNameNode(int id)
        {
            return Context.ProductCatalog
                .Where(c => c.ProductCatalogId == id)
                .AsEnumerable()?
                .Select(c => CatalogDllFunction(c.Parent, c.CatalogName))
                .FirstOrDefault();
        }

        public void DeleteCustom(int id)
        {
            var catalog = Context.ProductCatalog.FirstOrDefault(c => c.ProductCatalogId == id && !c.InverseParent.Any());

            if (catalog == null) throw new Exception("Delete the sub-catalogs first");
            Context.ProductCatalog.Remove(catalog);
        }

        public ProductCatalogUpdateViewModel FindForUpdate(int id)
        {
            return Context.ProductCatalog.Select(c => new ProductCatalogUpdateViewModel
            {
                ProductCatalogId = c.ProductCatalogId,
                CatalogName = c.CatalogName
            }).FirstOrDefault(c => c.ProductCatalogId == id);


        }

        public void UpdateCustom(ProductCatalogUpdateViewModel model)
        {
            var catalog = Context.ProductCatalog.Find(model.ProductCatalogId);

            if (Context.ProductCatalog.Any(c =>
                c.ParentId == catalog.ParentId && c.CatalogName == model.CatalogName &&
                c.ProductCatalogId != model.ProductCatalogId)) throw new Exception("Name already exists");
            catalog.CatalogName = model.CatalogName;
            Context.ProductCatalog.Update(catalog);
        }

        string CatalogDllFunction(ProductCatalog catalog, string cat)
        {

            if (catalog != null)
            {
                cat = CatalogDllFunction(catalog.Parent, catalog.CatalogName) + ">" + cat;
            }

            return cat;
        }
    }
}
