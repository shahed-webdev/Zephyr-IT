using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class AdminMoneyCollectionRepository : Repository<AdminMoneyCollection>, IAdminMoneyCollectionRepository
    {
        protected readonly IMapper _mapper;
        public AdminMoneyCollectionRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public DbResponse<AdminMoneyCollectionViewModel> Add(AdminMoneyCollectionAddModel model)
        {
            var adminMoneyCollection = _mapper.Map<AdminMoneyCollection>(model);
            Context.AdminMoneyCollection.Add(adminMoneyCollection);
            Context.SaveChanges();
            var viewModel = _mapper.Map<AdminMoneyCollectionViewModel>(adminMoneyCollection);

            return new DbResponse<AdminMoneyCollectionViewModel>(true, $"{viewModel.CollectionAmount} Amount Deposited Successfully", viewModel);
        }

        public DbResponse Delete(int id)
        {
            var adminMoneyCollection = Context.AdminMoneyCollection.Find(id);
            Context.AdminMoneyCollection.Remove(adminMoneyCollection);
            Context.SaveChanges();
            return new DbResponse(true, $"{adminMoneyCollection.CollectionAmount} Amount Deleted Successfully");
        }

        public bool IsNull(int id)
        {
            return !Context.AdminMoneyCollection.Any(r => r.AdminMoneyCollectionId == id);
        }

        public DataResult<AdminMoneyCollectionViewModel> List(DataRequest request)
        {
            return Context.AdminMoneyCollection
                .ProjectTo<AdminMoneyCollectionViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(a => a.InsertDate)
                .ToDataResult(request);
        }

        public AdminMoneyCollectionViewModel Get(int id)
        {
            return Context.AdminMoneyCollection
                .ProjectTo<AdminMoneyCollectionViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault(a => a.AdminMoneyCollectionId == id);
        }
    }
}