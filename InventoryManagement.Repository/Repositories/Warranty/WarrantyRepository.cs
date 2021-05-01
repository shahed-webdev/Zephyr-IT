using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class WarrantyRepository : Repository<Warranty>, IWarrantyRepository
    {
        protected readonly IMapper _mapper;
        public WarrantyRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public int GetNewSn()
        {
            var sn = 0;
            if (Context.Warranty.Any())
            {
                sn = Context.Warranty.Max(s => s == null ? 0 : s.WarrantySn);
            }

            return sn + 1;
        }

        public DbResponse<int> Acceptance(WarrantyAcceptanceModel model)
        {
            var warranty = _mapper.Map<Warranty>(model);
            warranty.AcceptanceDate = DateTime.Now.BdTime().Date;
            warranty.WarrantySn = GetNewSn();
            Context.Warranty.Add(warranty);
            Context.SaveChanges();



            return new DbResponse<int>(true, "Warranty Accepted Successfully", warranty.WarrantyId);
        }

        public bool IsInWarranty(int productStockId)
        {
            return Context.Warranty.Any(w => !w.IsDelivered && w.ProductStockId == productStockId);
        }

        public bool IsNull(int warrantyId)
        {
            return !Context.Warranty.Any(w => w.WarrantyId == warrantyId);
        }

        public DbResponse<WarrantyAcceptanceReceiptModel> AcceptanceReceipt(int warrantyId)
        {
            var warranty = Context.Warranty
                .Where(w => w.WarrantyId == warrantyId)
                .ProjectTo<WarrantyAcceptanceReceiptModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
            return new DbResponse<WarrantyAcceptanceReceiptModel>(true, "Success", warranty);
        }
    }
}