﻿using AutoMapper;
using InventoryManagement.Data;
using InventoryManagement.Repository;
using System;

namespace InventoryManagement.BusinessLogin
{
    public class WarrantyCore : CoreDependency, IWarrantyCore
    {
        public WarrantyCore(IUnitOfWork db, IMapper mapper) : base(db, mapper)
        {
        }

        public DbResponse<int> Acceptance(WarrantyAcceptanceModel model, string userName)
        {
            try
            {
                if (_db.Warranty.IsInWarranty(model.ProductStockId))
                    return new DbResponse<int>(false, $"Product already accepted in Warranty");

                var registrationId = _db.Registrations.GetRegID_ByUserName(userName);
                if (registrationId == 0)
                    return new DbResponse<int>(false, $"Invalid User");

                //Product Logs 
                var logs = new ProductLogAddModel
                {
                    ProductStockId = model.ProductStockId,
                    ActivityByRegistrationId = registrationId,
                    Details = $"Product Accepted for warranty",
                    LogStatus = ProductLogStatus.WarrantyAcceptance
                };

                _db.ProductLog.Add(logs);

                return _db.Warranty.Acceptance(model);

            }
            catch (Exception e)
            {
                return new DbResponse<int>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

        public DbResponse<WarrantyAcceptanceReceiptModel> AcceptanceReceipt(int warrantyId)
        {
            try
            {
                if (_db.Warranty.IsNull(warrantyId))
                    return new DbResponse<WarrantyAcceptanceReceiptModel>(false, "Not Found");

                return _db.Warranty.AcceptanceReceipt(warrantyId);
            }
            catch (Exception e)
            {
                return new DbResponse<WarrantyAcceptanceReceiptModel>(false, $"{e.Message}. {e.InnerException?.Message ?? ""}");
            }
        }

    }
}