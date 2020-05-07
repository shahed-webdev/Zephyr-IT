namespace InventoryManagement.Repository
{
    public class DbResponse<TObject> where TObject : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TObject Data { get; set; }
    }
}