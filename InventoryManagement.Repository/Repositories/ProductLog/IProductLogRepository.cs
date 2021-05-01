using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IProductLogRepository
    {
        void Add(ProductLogAddModel model);
        void AddList(List<ProductLogAddModel> models);


    }
}