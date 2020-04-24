namespace InventoryManagement.Repository
{
    public interface IAddCustom<in TObject> where TObject : class
    {
        void AddCustom(TObject model);
    }
}
