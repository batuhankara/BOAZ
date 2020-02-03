namespace BAOZ.Api.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {

        }
        public ApiResponse(object data, bool isError = false, ApiException apiException = null, string responseTime = null)
        {
            IsError = isError;
            Data = data;
            ApiException = apiException;
            ResponseTime = responseTime;
        }

        public bool IsError { get; set; }
        public object Data { get; set; }
        public string ResponseTime { get; set; }
        public ApiException ApiException { get; set; }
    }
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse()
        {

        }
        public ApiResponse(object data, bool isError = false, ApiException apiException = null, string responseTime = null) : base(data, isError, apiException, responseTime)
        {
        }
    }
}
