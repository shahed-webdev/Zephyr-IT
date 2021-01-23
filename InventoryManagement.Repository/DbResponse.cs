namespace InventoryManagement.Repository
{
    public class DbResponse
    {
        public DbResponse()
        {

        }
        public DbResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public DbResponse(bool isSuccess, string message, string fieldName)
        {
            IsSuccess = isSuccess;
            Message = message;
            FieldName = fieldName;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string FieldName { get; set; } = "dbCustomError";
    }
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
        public DbResponse(bool isSuccess, string message, TObject data, string fieldName)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            FieldName = fieldName;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string FieldName { get; set; }
        public TObject Data { get; set; }
    }
}