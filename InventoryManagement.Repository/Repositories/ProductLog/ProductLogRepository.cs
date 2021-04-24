using AutoMapper;
using InventoryManagement.Data;
using System.Collections.Generic;
using System.Linq;

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
    }
}