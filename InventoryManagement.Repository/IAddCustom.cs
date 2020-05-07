using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IAddCustom<in TObject> where TObject : class
    {
        void AddCustom(TObject model);
    }

    public interface IAddCustomAsync<TObject> where TObject : class
    {
        Task<DbResponse<TObject>> AddCustomAsync(TObject model);
    }
}
