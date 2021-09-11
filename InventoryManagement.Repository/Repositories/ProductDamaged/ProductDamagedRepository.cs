using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class ProductDamagedRepository : Repository<ProductDamaged>, IProductDamagedRepository
    {
        protected readonly IMapper _mapper;
        public ProductDamagedRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public DbResponse Add(ProductDamagedAddModel model)
        {
            var productDamaged = _mapper.Map<ProductDamaged>(model);
            productDamaged.DamagedDate = DateTime.Now.Date.BdTime();
            Context.ProductDamaged.Add(productDamaged);
            Context.SaveChanges();

            return new DbResponse(true, $"Product Damaged Added");
        }

        public DbResponse Delete(int productDamagedId)
        {
            var productDamaged = Context.ProductDamaged.Find(productDamagedId);
            Context.ProductDamaged.Remove(productDamaged);
            Context.SaveChanges();
            return new DbResponse(true, $"Deleted Successfully");
        }

        public int GetProductStockId(int productDamagedId)
        {
            return Context.ProductDamaged.FirstOrDefault(d => d.ProductDamagedId == productDamagedId)?.ProductStockId ?? 0;
        }

        public DataResult<ProductDamagedViewModel> List(DataRequest request)
        {
            return Context.ProductDamaged
                .ProjectTo<ProductDamagedViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(a => a.DamagedDate)
                .ToDataResult(request);
        }

        public decimal DamagedAmountDateWise(DateTime? fromDate, DateTime? toDate)
        {
            var sD = fromDate ?? new DateTime(1000, 1, 1);
            var eD = toDate ?? new DateTime(3000, 12, 31);
            return Context
                .ProductDamaged
                .Include(s => s.ProductStock)
                .ThenInclude(p => p.PurchaseList)
                .Where(r => r.DamagedDate <= eD && r.DamagedDate >= sD)
                .Sum(s => s.ProductStock.PurchaseList.PurchasePrice);
        }

        public ICollection<MonthlyAmount> MonthlyDamaged(int year)
        {
            var months = (from damaged in Context.ProductDamaged
                          where damaged.DamagedDate.Year == year
                          select new
                          {
                              MonthNumer = damaged.DamagedDate.Month,
                              DamagedAmount = damaged.ProductStock.PurchaseList.PurchasePrice
                          }
                ).GroupBy(e => new
                {
                    number = e.MonthNumer

                })
                .Select(g => new MonthlyAmount
                {
                    MonthNumber = g.Key.number,
                    Amount = g.Sum(s => s.DamagedAmount)
                })
                .ToList();



            return months;
        }
    }
}