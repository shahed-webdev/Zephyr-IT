using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using Microsoft.EntityFrameworkCore;
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

        public DbResponse<int> Delivery(WarrantyDeliveryModel model)
        {
            var warranty = Context.Warranty.Find(model.WarrantyId);

            warranty.ChangedProductCatalogId = model.ChangedProductCatalogId;
            warranty.DeliveryDescription = model.DeliveryDescription;
            warranty.ChangedProductName = model.ChangedProductName;
            warranty.ChangedProductCode = model.ChangedProductCode;

            warranty.DeliveryDate = DateTime.Today.BdTime().Date;
            warranty.IsDelivered = true;

            Context.Warranty.Update(warranty);
            Context.SaveChanges();
            return new DbResponse<int>(true, "Warranty Delivered Successfully", warranty.WarrantyId);
        }

        public bool IsInWarranty(int productStockId)
        {
            return Context.Warranty.Any(w => !w.IsDelivered && w.ProductStockId == productStockId);
        }

        public bool IsNull(int warrantyId)
        {
            return !Context.Warranty.Any(w => w.WarrantyId == warrantyId);
        }

        public DbResponse<WarrantyReceiptModel> Receipt(int warrantyId)
        {
            var warranty = Context.Warranty
                .Include(w => w.Selling)
                .ThenInclude(s => s.Customer)
                .Where(w => w.WarrantyId == warrantyId)
                .ProjectTo<WarrantyReceiptModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
            return new DbResponse<WarrantyReceiptModel>(true, "Success", warranty);
        }

        public DataResult<WarrantyListViewModel> List(DataRequest request)
        {
            var warranty = Context.Warranty
                .Include(w => w.Selling)
                .ThenInclude(s => s.Customer)
                .ProjectTo<WarrantyListViewModel>(_mapper.ConfigurationProvider)
                .ToDataResult(request);
            return warranty;
        }
    }
}