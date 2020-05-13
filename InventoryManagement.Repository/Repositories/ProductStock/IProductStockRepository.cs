﻿using InventoryManagement.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IProductStockRepository : IRepository<ProductStock>
    {
        Task<bool> IsExistAsync(string productStock);
        Task<List<ProductStock>> IsExistListAsync(List<string> stocks);

    }
}