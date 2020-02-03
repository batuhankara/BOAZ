namespace BAOZ.Api.Models
{
    public class RequestResponseLogModel
    {
        public ResponseLogModel ResponseModel { get; set; }
        public RequestLogModel RequestModel { get; set; }
        public string ResponseTime { get; set; }

    }
}
