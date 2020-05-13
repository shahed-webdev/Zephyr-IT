namespace InventoryManagement.Repository
{
    public class DbResponse<TObject>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TObject Data { get; set; }
    }
}