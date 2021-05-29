using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductLogRepository : Repository<ProductLog>, IProductLogRepository
    {
        protected readonly IMapper _mapper;
        public ProductLogRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public void Add(ProductLogAddModel model)
        {
            var log = _mapper.Map<ProductLog>(model);
            Context.ProductLog.Add(log);
            Context.SaveChanges();
        }

        public void AddList(List<ProductLogAddModel> models)
        {
            var logs = models.Select(l => _mapper.Map<ProductLog>(l)).ToList();
            Context.ProductLog.AddRange(logs);
            Context.SaveChanges();
        }

        public async Task<DbResponse<List<ProductLogViewModel>>> FindLogAsync(int productStockId)
        {
            var logs = await Context.
                ProductLog
                .Include(p => p.Selling)
                .Where(l => l.ProductStockId == productStockId)
                 .ProjectTo<ProductLogViewModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();
            return new DbResponse<List<ProductLogViewModel>>(true, "Success", logs);
        }

        public async Task<DbResponse<List<ProductLogViewModel>>> FindLogByCodeAsync(string code)
        {
            var logs = await Context.
                ProductLog
                .Include(p => p.Selling)
                .Include(p => p.ProductStock)
                .Where(l => l.ProductStock.ProductCode == code)
                .ProjectTo<ProductLogViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new DbResponse<List<ProductLogViewModel>>(true, "Success", logs);
        }
    }
}