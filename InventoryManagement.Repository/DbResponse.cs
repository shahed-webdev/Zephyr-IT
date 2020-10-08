namespace InventoryManagement.Repository
{
    public class DbResponse<TObject>
    {
        public DbResponse()
        {

        }
        public DbResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public DbResponse(bool isSuccess, string message, TObject data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TObject Data { get; set; }
    }

    public class DbResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}