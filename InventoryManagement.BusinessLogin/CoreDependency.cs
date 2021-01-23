using AutoMapper;
using InventoryManagement.Repository;

namespace InventoryManagement.BusinessLogin
{
    public class CoreDependency
    {
        protected readonly IUnitOfWork _db;
        protected readonly IMapper _mapper;

        public CoreDependency(IUnitOfWork db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }
    }
}