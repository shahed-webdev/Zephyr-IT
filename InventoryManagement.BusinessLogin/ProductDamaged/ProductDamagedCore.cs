using AutoMapper;
using InventoryManagement.Data;
using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using System;

namespace InventoryManagement.BusinessLogin
{
    public class ProductDamagedCore : CoreDependency, IProductDamagedCore
    {
        public ProductDamagedCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
        {
        }

        public DbResponse Add(ProductDamagedAddModel model, string userName)
        {
            try
            {
                if (!_db.ProductStocks.IsInStock(model.ProductStockId))
                    return new DbResponse(false, $"{model.ProductCode} Product is out of stock");

                var registrationId = _db.Registrations.GetRegID_ByUserName(userName);
                if (registrationId == 0)
                    return new DbResponse(false, $"Invalid User");

                //Product Logs 
                var logs = new ProductLogAddModel
                {
                    ProductStockId = model.ProductStockId,
                    ActivityByRegistrationId = registrationId,
                    Details = $"Product Damaged {model.Note}",
                    LogStatus = ProductLogStatus.Damaged
                };

                _db.ProductLog.Add(logs);

                _db.ProductStocks.StockOut(model.ProductStockId);

                return _db.ProductDamaged.Add(model);

            }
            catch (Exception e)
            {
                return new DbResponse(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse Delete(int productDamagedId, string userName)
        {
            try
            {
                var productStockId = _db.ProductDamaged.GetProductStockId(productDamagedId);

                if (productStockId == 0)
                    return new DbResponse(false, $"Not Found");

                if (_db.ProductStocks.IsInStock(productStockId))
                    return new DbResponse(false, $"Product is already in stock");

                var registrationId = _db.Registrations.GetRegID_ByUserName(userName);
                if (registrationId == 0)
                    return new DbResponse(false, $"Invalid User");

                //Product Logs 
                var logs = new ProductLogAddModel
                {
                    ProductStockId = productStockId,
                    ActivityByRegistrationId = registrationId,
                    Details = $"Product Damaged undo",
                    LogStatus = ProductLogStatus.Damaged
                };

                _db.ProductLog.Add(logs);

                _db.ProductStocks.StockIn(productStockId);

                return _db.ProductDamaged.Delete(productDamagedId);

            }
            catch (Exception e)
            {
                return new DbResponse(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DataResult<ProductDamagedViewModel> List(DataRequest request)
        {
            return _db.ProductDamaged.List(request);
        }

        public decimal DamagedAmountDateWise(DateTime? fromDate, DateTime? toDate)
        {
            return _db.ProductDamaged.DamagedAmountDateWise(fromDate, toDate);
        }
    }
}