using System.Collections.Generic;

namespace BAOZ.Api.Models
{
    public class ResponseLogModel
    {
        public string Body { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
    }
}
