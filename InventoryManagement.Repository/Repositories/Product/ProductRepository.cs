using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void AddCustom(ProductShowViewModel model)
        {
            var product = new Product
            {
                ProductCatalogId = model.ProductCatalogId,
                ProductName = model.ProductName,
                Description = model.Description,
                Warranty = model.Warranty
            };

            Add(product);
        }

        public Task<bool> IsExistAsync(string name, int categoryId, int updateId = 0)
        {
            var product = Context.Product.Where(p => p.ProductCatalogId == categoryId && p.ProductName == name);

            if (updateId != 0) product.Where(p => p.ProductId != updateId);
            return product.AnyAsync();
        }

        public Task<List<ProductShowViewModel>> FindByCategoryAsync(int categoryId = 0)
        {
            var products = Context.Product.Include(p => p.ProductStock).Select(p =>
                new ProductShowViewModel
                {
                    ProductId = p.ProductId,
                    ProductCatalogId = p.ProductCatalogId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Warranty = p.Warranty,
                    Note = p.Note,
                    SellingPrice = p.SellingPrice,
                    Stock = p.ProductStock.Count(s => !s.IsSold)
                });

            if (categoryId != 0)
                return products.Where(p => p.ProductCatalogId == categoryId).ToListAsync();


            return products.Take(20).ToListAsync();
        }

        public DataResult<ProductShowViewModel> FindDataTable(DataRequest request)
        {
            var products = Context.Product.Include(p => p.ProductStock).Select(p =>
                new ProductShowViewModel
                {
                    ProductId = p.ProductId,
                    ProductCatalogId = p.ProductCatalogId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Warranty = p.Warranty,
                    Note = p.Note,
                    SellingPrice = p.SellingPrice,
                    Stock = p.ProductStock.Count(s => !s.IsSold)
                });
            return products.ToDataResult(request);
        }

        public bool RemoveCustom(int id)
        {
            if (Context.ProductStock.Any(e => e.ProductId == id)) return false;
            Remove(Find(id));
            return true;
        }

        public Task<ProductShowViewModel> FindByIdAsync(int ProductId)
        {
            var product = Context.Product.Where(p => p.ProductId == ProductId).Select(p =>
                 new ProductShowViewModel
                 {
                     ProductId = p.ProductId,
                     ProductCatalogId = p.ProductCatalogId,
                     ProductName = p.ProductName,
                     Description = p.Description,
                     Warranty = p.Warranty,
                     Note = p.Note,
                     SellingPrice = p.SellingPrice
                 });

            return product.FirstOrDefaultAsync();
        }

        public DbResponse<ProductViewModel> ProductWithCodes(int productId)
        {
            var response = new DbResponse<ProductViewModel>();
            try
            {
                var product = Context.Product.Include(p => p.ProductCatalog).Include(p => p.ProductStock).Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    Description = p.Description,
                    Warranty = p.Warranty,
                    Note = p.Note,
                    SellingPrice = p.SellingPrice,
                    PurchasePrice = p.SellingPrice,
                    ProductStocks = p.ProductStock.Where(s => !s.IsSold).Select(s => new ProductStockViewModel
                    {
                        ProductCode = s.ProductCode
                    }).ToList(),
                    ProductName = p.ProductName,
                    ProductCatalogId = p.ProductCatalogId,
                    ProductCatalogName = p.ProductCatalog.CatalogName
                }).FirstOrDefault(p => p.ProductId == productId);

                if (product == null)
                {
                    response.Message = "No data Found";
                    response.IsSuccess = false;
                    return response;
                }

                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = product;
                return response;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public int GetLastPurchaseId(int productId)
        {
            return Context.PurchaseList.OrderByDescending(p => p.Purchase.InsertDate)
                .FirstOrDefault(p => p.ProductId == productId)?.PurchaseId ?? 0;
        }
    }
}