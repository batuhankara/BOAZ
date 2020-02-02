using System.Collections.Generic;

namespace BAOZ.Api.Models
{
    public class ResponseModel
    {
        public string Body { get; set; }
        public List<KeyValuePair<string,string>> Headers { get; set; }
    }
    public class RequestResponseModel
    {
        public ResponseModel ResponseModel { get; set; }
        public RequestModel RequestModel { get; set; }
        public string ResponseTime { get; set; }

    }
}
